using System;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ConventionBase))]
    // ReSharper disable once InconsistentNaming
    internal class ConventionBase_Should
    {
        private class ConventionBaseImpl : ConventionBase
        {
            protected override void Configure() { }
        }

        [Test]
        public void Return_Valid_Classes()
        {
            var sut = new ConventionBaseImpl();
            var cls = sut.AllClasses();
            cls.Should().Contain(o => (Type)((object[])o)[0] == typeof(BecauseAttribute));
            cls.Should().NotContain(o => (Type)((object[])o)[0] == typeof(ConventionBaseImpl));
        }
    }
}