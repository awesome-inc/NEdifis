using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ClassAndTestReferenceEachOther))]
    // ReSharper disable once InconsistentNaming
    class ClassAndTestReferenceEachOther_Should
    {
        // ReSharper disable once InconsistentNaming
        class Inner01 { }

        // ReSharper disable once InconsistentNaming
        [TestFixtureFor(typeof(Inner01))]
        class Inner01_Should { }

        // ReSharper disable once InconsistentNaming
        [TestFixtureFor(typeof(Inner03))]
        class Inner02_Should { }

        // ReSharper disable once InconsistentNaming
        [TestedBy(typeof(Inner01_Should))]
        class Inner03 { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new ClassAndTestReferenceEachOther();
            sut.Should().NotBeNull();
            sut.HintOnFail.Should().NotBeNullOrWhiteSpace();

            sut.FulfilsConvention(typeof(ClassAndTestReferenceEachOther)).Should().Be(true);
            sut.FulfilsConvention(typeof(ClassAndTestReferenceEachOther_Should)).Should().Be(true);

            sut.FulfilsConvention(typeof(Inner02_Should)).Should().Be(false);
            sut.FulfilsConvention(typeof(Inner03)).Should().Be(false);
        }

        [Test, Ticket(6, Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new ClassAndTestReferenceEachOther();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}