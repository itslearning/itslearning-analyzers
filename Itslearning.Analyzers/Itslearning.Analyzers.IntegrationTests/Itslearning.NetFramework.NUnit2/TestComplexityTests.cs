using NUnit.Framework;

namespace Itslearning.NetFramework.NUnit2
{
    [TestFixture]
    public class TestComplexityTests
    {
        [Test]
        public void NoConditionals_ShouldNotError()
        {
        }

        [TestCase]
        public void Conditionals_InsideCalledMethods_ShouldNotError()
        {
            NoTestMethod_AndEvenContainingConditionals_NoError();
        }

        public void NoTestMethod_AndEvenContainingConditionals_NoError()
        {
            if (true) { }
            switch (1) { case 1: break; }
            System.Console.Write(1 > 0 ? 1 : 0);
        }

        [Test]
        public void TestMethod_ContainsIfConditional_ShouldError()
        {
            if (true) { }
        }

        [TestCase]
        public void TestMethod_ContainsSwitchConditional_ShouldError()
        {
            switch (1) { case 1: break; }
        }

        public static readonly int[] TestSource = new int[] { 1, 2 };

        [TestCaseSource(nameof(TestSource))]
        public void TestMethod_ContainsConditionalExpression_ShouldError(int x)
        {
            System.Console.Write(x > 0 ? 1 : 0);
        }
    }
}
