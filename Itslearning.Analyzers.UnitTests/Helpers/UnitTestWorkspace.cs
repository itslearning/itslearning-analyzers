using Microsoft.CodeAnalysis.Diagnostics;

namespace Itslearning.Analyzers.Tests.Helpers
{
    /// <summary>
    /// Defines the project to be constructed and analyzed by Roslyn in unit tests.
    /// </summary>
    public class UnitTestWorkspace
    {
        public DiagnosticAnalyzer Analyzer { get; set; }

        public string ProjectName { get; set; }

        public string[] FileNames { get; set; }

        public string[] SourceFiles { get; set; }

        public UnitTestWorkspace WithSources(params string[] sources)
        {
            return new UnitTestWorkspace
            {
                Analyzer = Analyzer,
                ProjectName = ProjectName,
                SourceFiles = sources
            };
        }
    }
}