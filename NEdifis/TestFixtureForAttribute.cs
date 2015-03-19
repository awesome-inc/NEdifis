using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace NEdifis
{
    /// <summary>
    /// Attribute do define the test fixture which tests the class.
    /// Currently you can resharper and F12 to navigate to the test class directly
    /// </summary>
    public class TestFixtureForAttribute : TestFixtureAttribute
    {
        public TestFixtureForAttribute(Type testClass)
        {
            TestClass = testClass;
        }

        public Type TestClass { get; private set; }
    }
}