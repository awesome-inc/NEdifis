using System;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    public class AllClassesNeedATest : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; set; } = type => type.IsClass && !type.Name.EndsWith("_Should");

        public Func<Type, Type> GetFixtureTypeFor { get; set; } = type =>
        {
            var name = type.FullName;
            var p = name.IndexOf('`'); // cutof generics
            if (p > 0) name = name.Substring(0, p);

            var typeName = name + "_Should";
            var fixtureType = type.Assembly.GetType(typeName);
            fixtureType.Should().NotBeNull($"type '{type}' should have a corresponding test '{typeName}'");
            return fixtureType;
        };

        public Action<Type> VerifyIsFixture { get; set; } = type => type.Should().BeDecoratedWith<TestFixtureAttribute>();

        public void Verify(Type type)
        {
            var fixtureType = GetFixtureTypeFor(type);
            VerifyIsFixture(fixtureType);
        }
    }
}
