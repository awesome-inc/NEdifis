using System;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ConventionException))]
    // ReSharper disable InconsistentNaming
    internal class ConventionException_Should
    {
        [Test]
        public void Print_pretty_convention_error()
        {
            var convention = new AllClassesNeedATest();
            var checkedType = typeof(string);
            var innerException = new ArgumentException("test");
            var sut = new ConventionException(convention, checkedType, innerException);
            sut.Message.Should().Be($"Type '{checkedType}' breaks convention '{convention.GetType().Name}': {innerException.Message}");
        }
    }
}