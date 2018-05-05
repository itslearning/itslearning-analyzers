using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Itslearning.Analyzers.UnitTesting
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestComplexityAnalyzer : ItslearningUnitTestingAnalyzerBase
    {
        private static readonly Regex AllowedTestComment = 
            new Regex("\\s*//\\s*([Aa]rrange|[Aa]ct|[Aa]ssert)\\s*$", RegexOptions.Compiled);

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
            var testMethod = TryExtractTestMethod(context);
            if (testMethod == null)
            {
                return;
            }

            AnalyzeConditionals(context, testMethod);
            AnalyzeComments(context, testMethod);
        }

        private static void AnalyzeConditionals(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax testMethod)
        {
            var conditionalStatement = testMethod
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
                testMethod.Identifier.ToString()));
        }

        private void AnalyzeComments(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax testMethod)
        {
            var trivias = testMethod.Body
                .DescendantTrivia()
                .Where(t => t.Kind() == SyntaxKind.SingleLineCommentTrivia
                            || t.Kind() == SyntaxKind.MultiLineCommentTrivia);

            foreach (var trivia in trivias)
            {
                if (!AllowedTestComment.IsMatch(trivia.ToString()))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        Descriptors.Itsa1005_AllowedComments,
                        trivia.GetLocation(),
                        testMethod.Identifier.ToString()));
                }
            }

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