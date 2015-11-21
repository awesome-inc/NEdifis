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

            sut.Verify(typeof(AllClassesNeedATest));
            sut.Invoking(x => x.Verify(typeof(Does_Not_Have_A_Test))).ShouldThrow<AssertionException>();
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            typeof(AllClassesNeedATest).IsPublic.Should().BeTrue();
        }
    }
}