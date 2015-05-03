using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixture]
    public abstract class ConventionBase
    {
        private readonly Assembly _assembly;
        protected readonly List<IVerifyConvention> Conventions = new List<IVerifyConvention>();

        protected ConventionBase()
        {
            _assembly = GetType().Assembly;
        }

        public object[] AllClasses()
        {
            // ReSharper disable once CoVariantArrayConversion
            return _assembly
                .GetTypes()
                .Where(t => !typeof(ConventionBase).IsAssignableFrom(t) &&
                            !t.GetCustomAttributes<ExcludeFromConventionsAttribute>(true).Any() &&
                            !t.IsInterface &&
                            !IsCompilerGenerated(t) &&
                            !t.IsNested)
                .Select(t => new object[] { t })
                .ToArray();
        }

        protected static bool IsCompilerGenerated(Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(
                typeof(CompilerGeneratedAttribute), 
                false).Any();

            return hasCompilerGeneratedAttribute;
        }

        [TestFixtureSetUp]
        protected abstract void Configure();

        [Test]
        [TestCaseSource("AllClasses")]
        public void Check(Type cls)
        {
            var failMessage = new StringBuilder();
            var hasAtLeastOneIssue = false;

            foreach (var convention in Conventions)
            {
                var isValid = convention.FulfilsConvention(cls);
                if (isValid) continue;

                failMessage.Append(convention.HintOnFail);
                hasAtLeastOneIssue = true;
            }

            hasAtLeastOneIssue.Should().BeFalse(failMessage.ToString());
        }
    }
}
