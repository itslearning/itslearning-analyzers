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
            "Test file names should have a standardized format to distinguish them from test utility files.",
            HelpLinkUriFor("ITSA1000"));

        public static readonly DiagnosticDescriptor Itsa1001_TestClassNaming = new DiagnosticDescriptor(
            "ITSA1001",
            "Test class name should have a standard ending.",
            "Test class '{0}' has an invalid name.",
            "Naming",
            DiagnosticSeverity.Error,
            true,
            "Test class names should have a standardized format to distinguish them from test utility classes.",
            HelpLinkUriFor("ITSA1001"));

        public static readonly DiagnosticDescriptor Itsa1002_TestMethodNaming = new DiagnosticDescriptor(
            "ITSA1002",
            "Test method name should have a standard format.",
            "Test method '{0}' has an invalid name.",
            "Naming",
            DiagnosticSeverity.Error,
            true,
            "Test method names should have a standardized format to distinguish them from test utility classes.",
            HelpLinkUriFor("ITSA1002"));


        private static string HelpLinkUriFor(string id) => HelpUriBase + id;

        private const string HelpUriBase = "https://TODO/";
    }
}