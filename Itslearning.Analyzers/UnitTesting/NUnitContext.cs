using System.Linq;
using Microsoft.CodeAnalysis;

namespace Itslearning.Analyzers.UnitTesting
{
    public class NUnitContext
    {
        public NUnitContext(Compilation compilation)
        {
            IsPresent = compilation.ReferencedAssemblyNames.Any(id => id.Name == "nunit.framework");
            KnownTypes = new KnownTypes(compilation);
        }

        public bool IsPresent { get; }

        public KnownTypes KnownTypes { get; }
    }
}