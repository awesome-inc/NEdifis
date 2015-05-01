using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(AllClassesNeedATest))]
    // ReSharper disable once InconsistentNaming
    class AllClassesNeedATest_Should
    {
        // ReSharper disable once InconsistentNaming
        class Does_Not_Have_A_Test { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new AllClassesNeedATest();
            sut.Should().NotBeNull();
            sut.HintOnFail.Should().NotBeNullOrWhiteSpace();

            sut.FulfilsConvention(typeof(AllClassesNeedATest)).Should().Be(true);
            sut.FulfilsConvention(typeof(Does_Not_Have_A_Test)).Should().Be(false);
        }
    }
}