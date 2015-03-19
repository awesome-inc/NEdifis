using FluentAssertions;
using NUnit.Framework;

namespace NEdifis
{
    [TestFixtureFor(typeof(ContextFor<>))]
    // ReSharper disable once InconsistentNaming
    class ContextFor_Should
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        class Thing { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new ContextFor<Thing>();

            sut.Should().NotBeNull();

            var actual = sut.BuildSut();
            actual.Should().NotBeNull();
        }
    }
}