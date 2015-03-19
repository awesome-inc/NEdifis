using System;
using System.ComponentModel;

namespace NEdifis
{
    /// <summary>
    /// Attribute do define the test fixture which tests the class.
    /// Currently you can resharper and F12 to navigate to the test class directly
    /// </summary>
    public class TestedByAttribute : Attribute
    {
        public TestedByAttribute(Type testClass)
        {
            TestClass = testClass;
        }

        [Browsable(false)]
        public Type TestClass { get; private set; }
    }
}