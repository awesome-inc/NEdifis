using System;
using System.Reflection;
using FluentAssertions;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(ClassAndTestReferenceEachOther_Should))]
    public class ClassAndTestReferenceEachOther : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; }

        public void Verify(Type type)
        {
            if (type.Name.EndsWith("_Should"))
                VerifyTestClass(type);
            else
                VerifyClass(type);
        }

        private static void VerifyClass(Type expected)
        {
            var fixture = expected.GetCustomAttribute<TestedByAttribute>()?.Fixture;
            fixture.Should().NotBeNull($"'{expected}' should be decorated with '{nameof(TestedByAttribute)}'.");
            var actual = fixture.GetCustomAttribute<TestFixtureForAttribute>()?.ClassToTest;
            actual.Should().NotBeNull($"'{fixture}' should be decorated with '{nameof(TestFixtureForAttribute)}'.");
            actual.Should().Be(expected, "class and fixture should reference each other");
        }

        private static void VerifyTestClass(Type expected)
        {
            var classToTest = expected.GetCustomAttribute<TestFixtureForAttribute>()?.ClassToTest;
            classToTest.Should().NotBeNull($"'{expected}' should be decorated with '{nameof(TestFixtureForAttribute)}'.");
            var actual = classToTest.GetCustomAttribute<TestedByAttribute>()?.Fixture;
            actual.Should().NotBeNull($"'{classToTest}' should be decorated with '{nameof(TestedByAttribute)}'.");
            actual.Should().Be(expected, "class and fixture should reference each other");
        }
    }
}