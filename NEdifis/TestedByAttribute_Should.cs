using FluentAssertions;
using NUnit.Framework;

namespace NEdifis
{
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