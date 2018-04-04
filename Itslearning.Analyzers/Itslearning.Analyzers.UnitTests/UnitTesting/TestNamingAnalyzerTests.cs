using Itslearning.Analyzers.Tests.Helpers;
using Itslearning.Analyzers.UnitTesting;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using DiagnosticVerifier = Itslearning.Analyzers.Tests.Helpers.DiagnosticVerifier;

namespace Itslearning.Analyzers.Tests.UnitTesting
{
    [TestFixture]
    public class TestNamingAnalyzerTests : DiagnosticVerifier
    {
        protected override UnitTestWorkspace Workspace => new UnitTestWorkspace
        {
            Analyzer = new TestNamingAnalyzer()
        };

        [TestCase("Calculator.Test")]
        [TestCase("CalculatorTest")]
        [TestCase("CalculatorTests")]
        [TestCase("Calculator.Tests")]
        [TestCase("Calculator.Tests.Fixture1")]
        [TestCase("CalculatorIntegrationTests")]
        [TestCase("Calculator.IntegrationTests.Fixture1")]
        [TestCase("CalculatorUnitTests")]
        [TestCase("Calculator.UnitTests.Fixture1")]
        [TestCase("UnitTests.Calculator")]
        [TestCase("Tests.Calculator")]
        [TestCase("IntegrationTests.Calculator")]
        [TestCase("Test.Calculator")]
        public void ForTestProject_WhenProjectNameIsInvalid_ShouldReport(string incorrectProjectName)
        {
            var workspace = new UnitTestWorkspace
            {
                ProjectName = incorrectProjectName,
                SourceFiles = new [] { "" },
                Analyzer = new TestNamingAnalyzer()
            };

            Verify(workspace, new DiagnosticResult
            {
                Id = Descriptors.Itsa1003_TestProjectNaming.Id,
                Locations = new[] { new DiagnosticResultLocation() },
                Message = $"Test project '{incorrectProjectName}' has an invalid ending.",
                Severity = DiagnosticSeverity.Error
            });
        }

        [TestCase("Calculator.UnitTests")]
        [TestCase("Calculator.IntegrationTests")]
        [TestCase("Itslearning.Calculator.UnitTests")]
        [TestCase("Itslearning.Calculator.IntegrationTests")]
        public void ForTestProject_WhenProjectNameIsValid_ShouldNotReport(string correctProjectName)
        {
            var workspace = new UnitTestWorkspace
            {

                ProjectName = correctProjectName,
                SourceFiles = new[] { "" },
                Analyzer = new TestNamingAnalyzer()
            };

            Verify(workspace);
        }

        [TestCase("CalculatorTest.cs")]
        [TestCase("CalculatorFixture.cs")]
        [TestCase("CalculatorTests.Helpers.cs")]
        public void ForTestFile_WhenFileNameIsInvalid_ShouldReport(string incorrectFileName)
        {
            var workspace = new UnitTestWorkspace
            {
                FileNames = new [] {incorrectFileName},
                SourceFiles = new[] { "" },
                Analyzer = new TestNamingAnalyzer()
            };

            Verify(workspace, new DiagnosticResult
            {
                Id = Descriptors.Itsa1000_TestFileNaming.Id,
                Message = $"Test file '{incorrectFileName}' containing tests has an invalid name.",
                Severity = DiagnosticSeverity.Error
            });
        }

        [TestCase("CalculatorTests.cs")]
        [TestCase("CalculatorUnitTests.cs")]
        [TestCase("Tests.cs")]
        [TestCase("Calculator.Addition.Tests.cs")]
        public void ForTestFile_WhenFileNameIsValid_ShouldNotReport(string correctFileName)
        {
            var workspace = new UnitTestWorkspace
            {
                FileNames = new[] { correctFileName },
                SourceFiles = new[] { "" },
                Analyzer = new TestNamingAnalyzer()
            };

            Verify(workspace);
        }

        [TestCase("CalculatorFixture")]
        [TestCase("Calculator")]
        [TestCase("CalculatorTest")]
        public void ForTestClass_WhenClassNameIsInvalid_ShouldReport(string className)
        {
            var source = @"
[NUnit.Framework.TestFixture]
public class " + className + @"
{
}";
            Verify(source, new DiagnosticResult
            {
                Id = Descriptors.Itsa1001_TestClassNaming.Id,
                Locations = new [] {new DiagnosticResultLocation("Test0Tests.cs", 3, 14) },
                Message = $"Test class '{className}' has an invalid name.",
                Severity = DiagnosticSeverity.Error
            });
        }

        [TestCase("CalculatorTests")]
        [TestCase("CalculatorUnitTests")]
        public void ForTestClass_WhenClassNameIsValid_ShouldNotReport(string className)
        {
            var source = @"
[NUnit.Framework.TestFixture]
public class " + className + @"
{
}";
            Verify(source);
        }

        [Test]
        [Description("Testing to cover the case of avoiding attributes checking for struct types.")]
        public void ForTestStruct_WhenTypeNameIsInvalid_ShouldNotReport()
        {
            var source = @"
public struct CalculatorType
{
}";
            Verify(source);
        }

        [TestCase("TestMethod")]
        [TestCase("TestMethodShouldBeCorrect")]
        [TestCase("TestMethod_IsCorrect")]
        public void ForTestMethod_WhenMethodNameIsInvalid_ShouldReport(string methodName)
        {
            var source = @"
using NUnit.Framework;
[TestFixture]
public class CalculatorTests
{
    [Test]
    public void " + methodName + @"()
    {
    }

    [TestCase]
    public void TestCaseVariantOf_" + methodName + @"()
    {
    } 
}";
            Verify(source, 
                new DiagnosticResult
                {
                    Id = Descriptors.Itsa1002_TestMethodNaming.Id,
                    Locations = new [] {new DiagnosticResultLocation("Test0Tests.cs", 7, 17) },
                    Message = $"Test method '{methodName}' has an invalid name.",
                    Severity = DiagnosticSeverity.Error
                },
                new DiagnosticResult
                {
                    Id = Descriptors.Itsa1002_TestMethodNaming.Id,
                    Locations = new[] { new DiagnosticResultLocation("Test0Tests.cs", 12, 17) },
                    Message = $"Test method 'TestCaseVariantOf_{methodName}' has an invalid name.",
                    Severity = DiagnosticSeverity.Error
                });
        }

        [TestCase("TestMethod_ShouldReturnCorrectResult")]
        [TestCase("TestMethod_ShouldBeCorrect")]
        [TestCase("TestMethod_WhenSomething_ShouldBeCorrect")]
        [TestCase("TestMethod_WhenSomething_Should_BeCorrect")]
        public void ForTestMethod_WhenMethodNameIsValid_ShouldNotReport(string methodName)
        {
            var source = @"
using NUnit.Framework;
[TestFixture]
public class CalculatorTests
{
    [Test]
    public void " + methodName + @"()
    {
    }

    [TestCase]
    public void TestCaseVariantOf_" + methodName + @"()
    {
    }
}";
            Verify(source);
        }
    }
}