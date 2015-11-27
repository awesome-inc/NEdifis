using System;
using System.Reflection;
using FluentAssertions;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    public class TestClassesShouldEndWithShould : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; } =
            type => type.Name.EndsWith("_Should") || type.GetCustomAttribute<TestFixtureForAttribute>() != null;

        public void Verify(Type t)
        {
            if (t.Name.EndsWith("_Should"))
            {
                t.Should().BeDecoratedWith<TestFixtureForAttribute>();
                return;
            }

            var a = t.GetCustomAttribute<TestFixtureForAttribute>();
            if (a != null)
                t.Name.Should().EndWith("_Should");
        }
    }
}