using System;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    public class TestClassesShouldBePrivate : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; set; } = type => type.IsDecoratedWith<TestFixtureAttribute>();

        public void Verify(Type type)
        {
            type.IsPublic.Should().BeFalse($"test fixtures like '{type}' should not pe public");
        }
    }
}