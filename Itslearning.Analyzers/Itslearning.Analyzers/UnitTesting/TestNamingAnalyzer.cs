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
        private const string TestFileNameEnding = "Tests";
        private const string TestClassNameEnding = "Tests";
        private static readonly Regex MethodNameRegex = new Regex(@"\S+_Should\S+", RegexOptions.Compiled);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(new[]
        {
            Descriptors.Itsa1000_TestFileNaming,
            Descriptors.Itsa1001_TestClassNaming,
            Descriptors.Itsa1002_TestMethodNaming,
        });

        protected override void Analyze(CompilationStartAnalysisContext context)
        {
            context.RegisterCompilationEndAction(AnalyzeTestFileName);
            context.RegisterSymbolAction(AnalyzeTestClassName, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeTestMethodName, SymbolKind.Method);
        }

        private static void AnalyzeTestFileName(CompilationAnalysisContext compilationContext)
        {
            if (!compilationContext.Compilation.AssemblyName.EndsWith(TestFileNameEnding))
            {
                compilationContext.ReportDiagnostic(Diagnostic.Create(
                    Descriptors.Itsa1000_TestFileNaming,
                    Location.None,
                    compilationContext.Compilation.AssemblyName));
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
