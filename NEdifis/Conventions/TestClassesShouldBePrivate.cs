using System;
using FluentAssertions;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(TestClassesShouldBePrivate_Should))]
    public class TestClassesShouldBePrivate : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; set; } = type => type.Name.EndsWith("_Should");

        public void Verify(Type t)
        {
            t.IsPublic.Should().BeFalse();
        }
    }
}