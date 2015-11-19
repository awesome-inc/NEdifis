using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ClassAndTestReferenceEachOther))]
    // ReSharper disable once InconsistentNaming
    internal class ClassAndTestReferenceEachOther_Should
    {
        // ReSharper disable once InconsistentNaming
        private class Inner01 { }

        // ReSharper disable once InconsistentNaming
        [TestFixtureFor(typeof(Inner01))]
        private class Inner01_Should { }

        // ReSharper disable once InconsistentNaming
        [TestFixtureFor(typeof(Inner03))]
        private class Inner02_Should { }

        // ReSharper disable once InconsistentNaming
        [TestedBy(typeof(Inner01_Should))]
        private class Inner03 { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new ClassAndTestReferenceEachOther();

            sut.Verify(typeof(ClassAndTestReferenceEachOther));
            sut.Verify(typeof(ClassAndTestReferenceEachOther_Should));

            sut.Invoking(x => x.Verify(typeof(Inner02_Should))).ShouldThrow<AssertionException>();
            sut.Invoking(x => x.Verify(typeof(Inner03))).ShouldThrow<AssertionException>();
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new ClassAndTestReferenceEachOther();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}