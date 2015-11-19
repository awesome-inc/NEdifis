using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(TestClassesShouldBePrivate))]
    // ReSharper disable once InconsistentNaming
    internal class TestClassesShouldBePrivate_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new TestClassesShouldBePrivate();
            sut.Should().NotBeNull();
            sut.HintOnFail.Should().NotBeNullOrWhiteSpace();

            sut.FulfilsConvention(typeof(TestClassesShouldBePrivate_Should)).Should().Be(true);
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new TestClassesShouldBePrivate();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}