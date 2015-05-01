using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(ExcludeFromConventionsAttribute))]
    // ReSharper disable once InconsistentNaming
    class ExcludeFromConventionsAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new ExcludeFromConventionsAttribute();
            sut.Should().NotBeNull();
        }
    }
}