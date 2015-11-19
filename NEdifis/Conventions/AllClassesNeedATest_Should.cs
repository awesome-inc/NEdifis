using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(AllClassesNeedATest))]
    // ReSharper disable InconsistentNaming
    internal class AllClassesNeedATest_Should
    {
        private class Does_Not_Have_A_Test { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new AllClassesNeedATest();
            sut.Should().NotBeNull();
            sut.GetType().IsPublic.Should().BeTrue();
            sut.HintOnFail.Should().NotBeNullOrWhiteSpace();

            sut.FulfilsConvention(typeof(AllClassesNeedATest)).Should().Be(true);
            sut.FulfilsConvention(typeof(Does_Not_Have_A_Test)).Should().Be(false);
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new AllClassesNeedATest();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}