using System;
using System.Linq;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ConventionBase))]
    // ReSharper disable once InconsistentNaming
    internal class ConventionBase_Should
    {
        [Test]
        public void Return_Valid_Classes()
        {
            var sut = new ConventionBaseImpl();
            var cls = sut.AllClasses();
            cls.Should().Contain(o => (Type)((object[])o)[0] == typeof(BecauseAttribute));
            cls.Should().NotContain(o => (Type)((object[])o)[0] == typeof(ConventionBaseImpl));
        }

        [Test]
        public void Use_convention_filter()
        {
            var convention = Substitute.For<IVerifyConvention>();
            convention.Filter.Returns(type => type == typeof(string));
            convention.When(x => x.Verify(typeof(string))).Do(x => { throw new AssertionException("test");});

            var sut = new ConventionBaseImpl(convention);
            sut.Invoking(x => x.Check(typeof(string))).ShouldThrow<AssertionException>();
            sut.Check(typeof(int));
            var temp = convention.Received(2).Filter;
        }

        private class ConventionBaseImpl : ConventionBase
        {
            public ConventionBaseImpl(params IVerifyConvention[] conventions)
            {
                if (conventions != null && conventions.Any())
                    Conventions.AddRange(conventions);
            }
        }
    }
}