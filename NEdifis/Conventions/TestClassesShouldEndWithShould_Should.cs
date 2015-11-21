using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(TestClassesShouldEndWithShould))]
    // ReSharper disable once InconsistentNaming
    internal class TestClassesShouldEndWithShould_Should
    {
        [TestFixtureFor(typeof(TestClassesShouldEndWithShould))]
        // ReSharper disable once InconsistentNaming
        private class I_Am_A_Test_WithoutShould { }

        // ReSharper disable once InconsistentNaming
        private class Am_A_Should_Without_TestFixtureFor_Should { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new TestClassesShouldEndWithShould();

            sut.Verify(typeof(TestClassesShouldEndWithShould));
            sut.Invoking(x => x.Verify(typeof(I_Am_A_Test_WithoutShould))).ShouldThrow<AssertionException>();
            sut.Invoking(x => x.Verify(typeof(Am_A_Should_Without_TestFixtureFor_Should))).ShouldThrow<AssertionException>();
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new TestClassesShouldEndWithShould();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}