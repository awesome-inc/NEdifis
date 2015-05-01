using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(TestClassesShouldBePrivate))]
    // ReSharper disable once InconsistentNaming
    class TestClassesShouldBePrivate_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new TestClassesShouldBePrivate();
            sut.Should().NotBeNull();
            sut.HintOnFail.Should().NotBeNullOrWhiteSpace();

            sut.FulfilsConvention(typeof(TestClassesShouldBePrivate_Should)).Should().Be(true);
        }
    }
}