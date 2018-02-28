using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(TestClassesShouldMatchClassToTest))]
    // ReSharper disable once InconsistentNaming
    internal class TestClassesShouldMatchClassToTest_Should
    {
        [TestFixtureFor(typeof(TestClassesShouldMatchClassToTest))]
        // ReSharper disable once InconsistentNaming
        private class I_Am_A_Test_WithoutShould { }

        // ReSharper disable once InconsistentNaming
        private class Am_A_Should_Without_TestFixtureFor_Should { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new TestClassesShouldMatchClassToTest();

            sut.Verify(typeof(TestClassesShouldMatchClassToTest_Should));
            sut.Invoking(x => x.Verify(typeof(I_Am_A_Test_WithoutShould))).Should().Throw<AssertionException>();
            sut.Invoking(x => x.Verify(typeof(Am_A_Should_Without_TestFixtureFor_Should))).Should().Throw<AssertionException>();
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new TestClassesShouldMatchClassToTest();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}
