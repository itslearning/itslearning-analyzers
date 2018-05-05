using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Itslearning.Analyzers
{
    /// <summary>
    /// ITSA1* - unit test standards analysis.
    /// ITSA2* - general coding convention.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Descriptors
    {
        public static readonly DiagnosticDescriptor Itsa1000_TestFileNaming = new DiagnosticDescriptor(
            "ITSA1000",
            "Test file name should have a standard ending.",
            "Test file '{0}' containing tests has an invalid name.",
            "Naming",
            DiagnosticSeverity.Error,
            true,
            "Test file names should have a standardized format to distinguish them from test utility files. " +
            "The allowed format is '*Tests.cs' for every file that contains one or more test fixture classes.",
            HelpLinkUriFor("ITSA1000"));

        public static readonly DiagnosticDescriptor Itsa1001_TestClassNaming = new DiagnosticDescriptor(
            "ITSA1001",
            "Test class name should have a standard ending.",
            "Test class '{0}' has an invalid name.",
            "Naming",
            DiagnosticSeverity.Error,
            true,
            "Test class names should have a standardized format to distinguish them from test utility classes." +
            "The allowed format is '*Tests' for every class that is marked a test fixture.",
            HelpLinkUriFor("ITSA1001"));
            
        public static readonly DiagnosticDescriptor Itsa1002_TestMethodNaming = new DiagnosticDescriptor(
            "ITSA1002",
            "Test method name should have a standard format.",
            "Test method '{0}' has an invalid name.",
            "Naming",
            DiagnosticSeverity.Error,
            true,
            "Test method names should have a standardized format to distinguish them from test utility classes. The allowed format is '{PRECONDITIONS}_Should{EFFECT}' for every method marked as a test method.",
            HelpLinkUriFor("ITSA1002"));

        public static readonly DiagnosticDescriptor Itsa1003_TestProjectNaming = new DiagnosticDescriptor(
            "ITSA1003",
            "Test project name should have a standard ending.",
            "Test project '{0}' has an invalid ending.",
            "Naming",
            DiagnosticSeverity.Error,
            true,
            "Test project names should have a standardized format to distinguish them from test utility files. The allowed format is: '*.(Unit|Integration)Tests' for every project that contains test fixtures.",
            HelpLinkUriFor("ITSA1003"));

        public static readonly DiagnosticDescriptor Itsa1004_ConditionalsInTestBodies = new DiagnosticDescriptor(
            "ITSA1004",
            "Test method should not contain conditional statements like if, switch etc.",
            "Test method '{0}' contains a conditional statement.",
            "Design",
            DiagnosticSeverity.Error,
            true,
            "Test methods should not contain complex logic that is the subject of testing itself. Try splitting " +
            "particular cases into dedicated test methods.",
            HelpLinkUriFor("ITSA1003"));

        public static readonly DiagnosticDescriptor Itsa1005_AllowedComments = new DiagnosticDescriptor(
            "ITSA1005",
            "Test method should not contain any other comment than '// arrange', '// act', '// assert'.",
            "Test method '{0}' contains not allowed comment.",
            "Maintainability",
            DiagnosticSeverity.Warning,
            true,
            "Commenting test steps indicates substantial test method complexity. Unit tests should be simple. " +
            "Consider extracting arrangement steps into some test utility.",
            HelpLinkUriFor("ITSA1003"));


        private static string HelpLinkUriFor(string id) => HelpUriBase + id;

        private const string HelpUriBase = "https://TODO/";
    }
}