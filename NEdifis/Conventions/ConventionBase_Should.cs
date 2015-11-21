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
            var types = ConventionBase.ClassesToTestFor<ConventionBase_Should>().ToArray();
            types.Should().Contain(typeof(BecauseAttribute));
            types.Should().NotContain(typeof(ConventionBase));
        }

        [Test]
        public void Support_customizing_conventions()
        {
            var convention1 = Substitute.For<IVerifyConvention>();
            var sut = new TestConvention();
            sut.Conventions.Add(convention1);

            var convention2 = Substitute.For<IVerifyConvention>();
            sut.Conventions.Add(convention2);

            sut.Conventions.Should().ContainInOrder(convention1, convention2);
            sut.Conventions.Should().ContainSingle(c => c.GetType() == typeof(AllClassesNeedATest));
        }


        private class TestConvention : ConventionBase
        {
            public TestConvention()
            {
                Conventions.Add(new AllClassesNeedATest());
            }
        }
    }
}
