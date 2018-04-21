using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Itslearning.Analyzers.UnitTesting
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestComplexityAnalyzer : ItslearningUnitTestingAnalyzerBase
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(new[]
            {
                Descriptors.Itsa1004_ConditionalsInTestBodies,
                Descriptors.Itsa1005_AllowedComments,
            });
        
        protected override void Analyze(CompilationStartAnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeTestMethodByAttribute, SyntaxKind.Attribute);
        }

        private void AnalyzeTestMethodByAttribute(SyntaxNodeAnalysisContext context)
        {
            var conditionalStatement = TryExtractTestMethod(context)?
                .DescendantNodes()
                .FirstOrDefault(n => n is IfStatementSyntax 
                                     || n is SwitchStatementSyntax
                                     || n is ConditionalExpressionSyntax);

            if (conditionalStatement == null)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(
                Descriptors.Itsa1004_ConditionalsInTestBodies, 
                conditionalStatement.GetLocation(),
                TryExtractTestMethod(context).Identifier.ToString()));
        }

        private MethodDeclarationSyntax TryExtractTestMethod(SyntaxNodeAnalysisContext context)
        {
            var attribute = (AttributeSyntax) context.Node;
            var attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol?.ContainingSymbol;
            if (attributeSymbol == null
                || !NUnitContext.KnownTypes.TestMethodMarkers.Contains(attributeSymbol)
                || !(attribute.Parent is AttributeListSyntax)
                || !(attribute.Parent.Parent is MethodDeclarationSyntax testMethod))
            {
                return null;
            }
            
            return testMethod;
        }
    }
}