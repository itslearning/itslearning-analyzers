using Itslearning.Analyzers.Tests.Helpers;
using Itslearning.Analyzers.UnitTesting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using DiagnosticVerifier = Itslearning.Analyzers.Tests.Helpers.DiagnosticVerifier;

namespace Itslearning.Analyzers.Tests.UnitTesting
{
    [TestFixture]
    public class TestNamingAnalyzerTests : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer() => new TestNamingAnalyzer();
        
        // TODO: assembly naming

        [Test]
        public void ForTestClass_WhenClassNameIsInvalid_Reports()
        {
            var source = @"
[NUnit.Framework.TestFixture]
public class CalculatorFixture
{
}";
            VerifyCSharpDiagnostic(source, new DiagnosticResult
            {
                Id = Descriptors.Itsa1001_TestClassNaming.Id,
                Locations = new [] {new DiagnosticResultLocation("Test0.cs", 3, 14) },
                Message = "Test class 'CalculatorFixture' has an invalid name.",
                Severity = DiagnosticSeverity.Error
            });
        }

        [Test]
        public void ForTestClass_WhenClassNameIsValid_DoesNotReport()
        {
            var source = @"
[NUnit.Framework.TestFixture]
public class CalculatorTests
{
}";
            VerifyCSharpDiagnostic(source);
        }

        [Test]
        public void ForTestMethod_WhenMethodNameIsInvalid_Reports()
        {
            var source = @"
using NUnit.Framework;
[TestFixture]
public class CalculatorTests
{
    [Test]
    public void TestMethod()
    {
    }   
}";
            VerifyCSharpDiagnostic(source, new DiagnosticResult
            {
                Id = Descriptors.Itsa1002_TestMethodNaming.Id,
                Locations = new [] {new DiagnosticResultLocation("Test0.cs", 7, 17) },
                Message = "Test method 'TestMethod' has an invalid name.",
                Severity = DiagnosticSeverity.Error
            });
        }

        [Test]
        public void ForTestMethod_WhenMethodNameIsValid_DoesNotReport()
        {
            var source = @"
using NUnit.Framework;
[TestFixture]
public class CalculatorTests
{
    [Test]
    public void TestMethod_ShouldReturnCorrectResult()
    {
    }   
}";
            VerifyCSharpDiagnostic(source);
        }
    }
}