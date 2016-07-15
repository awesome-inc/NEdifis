using System;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    /// <summary>
    /// Attribute do define the test fixture which tests the class.
    /// Currently you can resharper and F12 to navigate to the test class directly
    /// </summary>
    public class TestFixtureForAttribute : TestFixtureAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixtureForAttribute"/>.
        /// </summary>
        /// <param name="classToTest">The type to test</param>
        /// <exception cref="ArgumentNullException">Thrown, when the specified type is null</exception>
        public TestFixtureForAttribute(Type classToTest)
        {
            if (classToTest == null) throw new ArgumentNullException(nameof(classToTest));
            ClassToTest = classToTest;
        }

        /// <summary>
        /// The type to test.
        /// </summary>
        public Type ClassToTest { get; private set; }
    }
}