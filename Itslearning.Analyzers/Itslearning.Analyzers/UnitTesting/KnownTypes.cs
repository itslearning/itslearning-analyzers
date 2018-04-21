using System.Collections.Immutable;
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
            TestCaseSource = compilation.GetTypeByMetadataName("NUnit.Framework.TestCaseSourceAttribute");

            if (TestFixture != null
                && Test != null
                && TestCase != null)
            {
                TestMethodMarkers = new ISymbol[] {Test, TestCase, TestCaseSource}.ToImmutableHashSet();
            }
            else
            {
                TestMethodMarkers = ImmutableHashSet<ISymbol>.Empty;
            }
            
        }

        public INamedTypeSymbol TestFixture { get; }

        public INamedTypeSymbol Test { get; }

        public INamedTypeSymbol TestCase { get; }
        
        public INamedTypeSymbol TestCaseSource { get; }
        
        /// <summary>
        /// Attributes that indicate a method being a unit test.
        /// </summary>
        public IImmutableSet<ISymbol> TestMethodMarkers { get; }
    }
}