using System;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis
{
    [TestFixtureFor(typeof(BecauseAttribute))]
    // ReSharper disable once InconsistentNaming
    class BecauseAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new BecauseAttribute("because why");

            sut.Reason.Should().Be("because why");

            true.Invoking(b => new BecauseAttribute(null)).ShouldThrow<ArgumentNullException>();
            true.Invoking(b => new BecauseAttribute(string.Empty)).ShouldThrow<ArgumentException>();
        }
    }
}