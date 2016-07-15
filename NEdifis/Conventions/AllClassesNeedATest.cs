using System;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    /// <summary>
    /// The 'all class should have a paring fixture' convention.
    /// </summary>
    public class AllClassesNeedATest : IVerifyConvention
    {
        /// <summary>
        /// A filter to select types where this convention can be applied.
        /// By default this selects classes that are not fixtures themselves.
        /// </summary>
        public Func<Type, bool> Filter { get; set; } = type => type.IsClass && !type.Name.EndsWith("_Should");

        /// <summary>
        /// A func to select the pairing fixture type for the specified type. 
        /// By default this assumes a fixture type ending with '_Should' in the same assembly and namespace 
        /// as the type to test.
        /// </summary>
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

        /// <summary>
        /// An action to assert the selected pairing type really is a fixture.
        /// By default this checks that the type is decorated with <see cref="TestFixtureAttribute"/>
        /// so that the NUnit test runner picks it up during tests.
        /// </summary>
        public Action<Type> VerifyIsFixture { get; set; } = type => type.Should().BeDecoratedWith<TestFixtureAttribute>();

        /// <summary>
        /// Asserts the convention on the specified type.
        /// </summary>
        /// <param name="type">The type to check</param>
        public void Verify(Type type)
        {
            var fixtureType = GetFixtureTypeFor(type);
            VerifyIsFixture(fixtureType);
        }
    }
}
