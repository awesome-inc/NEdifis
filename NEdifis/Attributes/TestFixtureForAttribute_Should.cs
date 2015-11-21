using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(TestFixtureForAttribute))]
    // ReSharper disable once InconsistentNaming
    internal class TestFixtureForAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new TestFixtureForAttribute(typeof(object));

            sut.ClassToTest.Should().Be<object>();
        }
    }
}