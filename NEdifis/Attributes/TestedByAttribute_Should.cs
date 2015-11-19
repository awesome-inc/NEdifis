using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(TestedByAttribute))]
    // ReSharper disable once InconsistentNaming
    internal class TestedByAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new TestedByAttribute(typeof(object));

            sut.Fixture.Should().Be<object>();
        }
    }
}