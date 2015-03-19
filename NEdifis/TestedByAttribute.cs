using System;
using System.ComponentModel;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis
{
    /// <summary>
    /// Attribute do define the test fixture which tests the class.
    /// Currently you can resharper and F12 to navigate to the test class directly
    /// </summary>
    [TestedBy(typeof(TestedByAttribute_Should))]
    public class TestedByAttribute : Attribute
    {
        public TestedByAttribute(Type testClass)
        {
            TestClass = testClass;
        }

        [Browsable(false)]
        public Type TestClass { get; private set; }
    }

    [TestFixtureFor(typeof(TestedByAttribute))]
    // ReSharper disable once InconsistentNaming
    class TestedByAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new TestedByAttribute(typeof(object));

            sut.TestClass.Should().Be<object>();
        }
    }
}