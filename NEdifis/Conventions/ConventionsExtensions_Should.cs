using System;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof (ConventionsExtensions))]
    // ReSharper disable InconsistentNaming
    internal class ConventionsExtensions_Should
    {
        [Test]
        public void Use_convention_filter()
        {
            var convention = Substitute.For<IVerifyConvention>();
            convention.Check(typeof(string));
            // ReSharper disable once UnusedVariable
            var temp = convention.Received().Filter;
        }

        [Test]
        public void Pretty_print_convention_failures()
        {
            var convention = Substitute.For<IVerifyConvention>();
            var typeToTest = typeof(string);
            convention.Filter.Returns(t => t == typeToTest);
            const string error = "wrong type";
            Exception exception = new ArgumentException(error);
            convention.When(x => x.Verify(typeToTest)).Do(x => throw exception);

            convention.Invoking(x => x.Check(typeToTest))
                .Should().Throw<ConventionException>()
                .WithMessage($"Type '{typeToTest}' breaks convention '{convention.GetType().Name}': {error}");

            exception = new ConventionException(convention, typeToTest, exception);

            convention.Invoking(x => x.Check(typeToTest))
                .Should().Throw<ConventionException>()
                .WithMessage($"Type '{typeToTest}' breaks convention '{convention.GetType().Name}': {error}");
        }

        [Test]
        public void Pretty_print_assertion_failures_on_conventions()
        {
            var convention1 = Substitute.For<IVerifyConvention>();
            var typeToTest = typeof(string);
            convention1.Filter.Returns(t => t == typeToTest);
            convention1.When(x => x.Verify(typeToTest)).Do(x => throw new InvalidCastException("test"));
            var conventions = new[] {convention1};

            conventions.Invoking(x => x.Check(typeToTest))
                .Should().Throw<AssertionException>()
                .WithMessage($"Some conventions were broken:\r\n  1. Type '{typeToTest}' breaks convention '{convention1.GetType().Name}': test");
        }
    }
}
