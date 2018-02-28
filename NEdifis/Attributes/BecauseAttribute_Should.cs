using System;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(BecauseAttribute))]
    // ReSharper disable once InconsistentNaming
    internal class BecauseAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var sut = new BecauseAttribute("because why");

            sut.Reason.Should().Be("because why");

            // ReSharper disable ObjectCreationAsStatement
            true.Invoking(b => new BecauseAttribute(null)).Should().Throw<ArgumentNullException>();
            true.Invoking(b => new BecauseAttribute(string.Empty)).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}
