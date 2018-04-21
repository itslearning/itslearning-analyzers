using Itslearning.Analyzers.Tests.Helpers;
using Itslearning.Analyzers.UnitTesting;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Itslearning.Analyzers.Tests.UnitTesting
{
    [TestFixture]
    public class TestComplexityAnalyzerTests : DiagnosticVerifier
    {
        protected override UnitTestWorkspace Workspace => new UnitTestWorkspace
        {
            Analyzer = new TestComplexityAnalyzer()
        };

        [TestCase("if (true) {}")]
        [TestCase("switch (1) { case 1: break; }", "[TestCaseSource(\"\")]")]
        [TestCase("var x = 1 > 0 ? 1 : 0; System.Console.WriteLine(x);", "[TestCase]", 17)]
        public void ForTestMethod_WhenContainsConditionalStatement_ShouldReport(
            string conditional,
            string testMethodMarker = "[Test]",
            int column = 9)
        {
            var source = @"
using NUnit.Framework;

[TestFixture]
public class Tests
{
    " + testMethodMarker + @"
    public void Test_WithConditional_ShouldProduceError()
    {
        " + conditional + @"
    }
}";
            Verify(source, new DiagnosticResult
            {
                Id = Descriptors.Itsa1004_ConditionalsInTestBodies.Id,
                Locations = new[] {new DiagnosticResultLocation("Test0Tests.cs", 10, column)},
                Message = "Test method 'Test_WithConditional_ShouldProduceError' contains a conditional statement.",
                Severity = DiagnosticSeverity.Error
            });
        }

        [TestCase("if (true) {}")]
        [TestCase("switch (1) { case 1: break; }")]
        [TestCase("var x = 1 > 0 ? 1 : 0; System.Console.WriteLine(x);")]
        public void ForNonTestMethod_WhenContainsConditionalStatement_ShouldNotReport(string conditional)
        {
            var source = @"
using NUnit.Framework;

[TestFixture]
public class Tests
{
    public void SomeUtilityMethod_ThisShouldNotProduceError()
    {
        " + conditional + @"
    }
}";
            Verify(source);
        }

        [Test]
        public void ForTestMethod_WhenDoesNotContainConditionalStatement_ShouldNotReport()
        {
            var source = @"
using NUnit.Framework;

[TestFixture]
public class Tests
{
    [Test]
    public void Test_WithoutConditional_ShouldNotProduceError()
    {
    }
}";
            Verify(source);

        }
    }
}