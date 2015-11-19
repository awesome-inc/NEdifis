using System;

namespace NEdifis.Attributes
{
    /// <summary>
    /// Attribute do define the test fixture which tests the class.
    /// Currently you can resharper and F12 to navigate to the test class directly
    /// </summary>
    [TestedBy(typeof(TestedByAttribute_Should))]
    public class TestedByAttribute : Attribute
    {
        public TestedByAttribute(Type fixture)
        {
            Fixture = fixture;
        }

        public Type Fixture { get; private set; }
    }
}