using System;
using FluentAssertions;

namespace NEdifis.Conventions
{
    public class TestClassesShouldBePrivate : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; set; } = type => type.Name.EndsWith("_Should");

        public void Verify(Type type)
        {
            type.IsPublic.Should().BeFalse($"test fixtures like '{type}' should not pe public");
        }
    }
}