using Microsoft.CodeAnalysis;

namespace Itslearning.Analyzers.UnitTesting
{
    public class KnownTypes
    {
        public KnownTypes(Compilation compilation)
        {
            TestFixture = compilation.GetTypeByMetadataName("NUnit.Framework.TestFixtureAttribute");
            Test = compilation.GetTypeByMetadataName("NUnit.Framework.TestAttribute");
            TestCase = compilation.GetTypeByMetadataName("NUnit.Framework.TestCaseAttribute");
        }

        public INamedTypeSymbol TestFixture { get; }

        public INamedTypeSymbol Test { get; }

        public INamedTypeSymbol TestCase { get; }
    }
}