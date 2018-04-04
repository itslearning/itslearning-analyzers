using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Itslearning.Analyzers.UnitTesting
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestNamingAnalyzer : ItslearningUnitTestingAnalyzerBase
    {
        private const string IntegrationTestProjectNameEnding = ".IntegrationTests";
        private const string UnitTestProjectNameEnding = ".UnitTests";
        private const string TestFileNameEnding = "Tests.cs";
        private const string TestClassNameEnding = "Tests";
        private static readonly Regex MethodNameRegex = new Regex(@"\S+_Should\S+", RegexOptions.Compiled);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(new[]
        {
            Descriptors.Itsa1000_TestFileNaming,
            Descriptors.Itsa1001_TestClassNaming,
            Descriptors.Itsa1002_TestMethodNaming,
            Descriptors.Itsa1003_TestProjectNaming,
        });

        protected override void Analyze(CompilationStartAnalysisContext context)
        {
            context.RegisterCompilationEndAction(AnalyzeTestProjectName);
            context.RegisterCompilationEndAction(AnalyzeTestFileName);
            context.RegisterSymbolAction(AnalyzeTestClassName, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeTestMethodName, SymbolKind.Method);
        }

        private static void AnalyzeTestProjectName(CompilationAnalysisContext compilationContext)
        {
            var projectName = compilationContext.Compilation.AssemblyName;

            if (!projectName.EndsWith(IntegrationTestProjectNameEnding)
                && !projectName.EndsWith(UnitTestProjectNameEnding))
            {
                compilationContext.ReportDiagnostic(Diagnostic.Create(
                    Descriptors.Itsa1003_TestProjectNaming,
                    Location.None,
                    projectName));
            }
        }

        private static void AnalyzeTestFileName(CompilationAnalysisContext compilationContext)
        {
            var fileNames = compilationContext.Compilation.SyntaxTrees
                .Where(st => st.FilePath != null).Select(st => st.FilePath);

            foreach (var fileName in fileNames)
            {
                if (!fileName.EndsWith(TestFileNameEnding))
                {
                    compilationContext.ReportDiagnostic(Diagnostic.Create(
                        Descriptors.Itsa1000_TestFileNaming,
                        Location.None,
                        fileName));
                }
            }
        }

        private void AnalyzeTestClassName(SymbolAnalysisContext symbolContext)
        {
            var namedType = (INamedTypeSymbol) symbolContext.Symbol;
            if (namedType.IsValueType)
                return;

            var isTestClass = namedType
                .GetAttributes()
                .Any(a => a.AttributeClass.Equals(NUnitContext.KnownTypes.TestFixture));

            if (isTestClass && !namedType.Name.EndsWith(TestClassNameEnding))
            {
                symbolContext.ReportDiagnostic(Diagnostic.Create(
                    Descriptors.Itsa1001_TestClassNaming,
                    symbolContext.Symbol.Locations.First(),
                    namedType.Name));
            }
        }

        private void AnalyzeTestMethodName(SymbolAnalysisContext symbolContext)
        {
            var method = (IMethodSymbol) symbolContext.Symbol;

            var isTestMethod = method.GetAttributes().Any(a => 
                a.AttributeClass.Equals(NUnitContext.KnownTypes.Test)
                || a.AttributeClass.Equals(NUnitContext.KnownTypes.TestCase));

            if (isTestMethod && !MethodNameRegex.IsMatch(method.Name))
            {
                symbolContext.ReportDiagnostic(Diagnostic.Create(
                    Descriptors.Itsa1002_TestMethodNaming,
                    symbolContext.Symbol.Locations.First(),
                    method.Name));
            }
        }
    }
}
