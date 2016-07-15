using System;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    /// <summary>
    /// The convention that test fixtures should be private (or internal) so that they don't affect runtime behavior.
    /// </summary>
    public class TestClassesShouldBePrivate : IVerifyConvention
    {
        /// <summary>
        /// A filter to select types where this convention can be applied.
        /// By default, this selects <see cref="TestFixtureAttribute"/>
        /// </summary>
        public Func<Type, bool> Filter { get; set; } = type => type.IsDecoratedWith<TestFixtureAttribute>();

        /// <summary>
        /// Asserts the convention on the specified type.
        /// </summary>
        /// <param name="type">The type to check</param>
        public void Verify(Type type)
        {
            type.IsPublic.Should().BeFalse($"test fixtures like '{type}' should not pe public");
        }
    }
}