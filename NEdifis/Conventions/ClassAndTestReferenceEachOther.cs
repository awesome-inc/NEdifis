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

        private static void VerifyClass(MemberInfo type)
        {
            var fixture = type.GetCustomAttribute<TestedByAttribute>().Fixture;
            var classToTest = fixture.GetCustomAttribute<TestFixtureForAttribute>().ClassToTest;
            type.Should().Be(classToTest);
        }

        private static void VerifyTestClass(MemberInfo type)
        {
            var classToTest = type.GetCustomAttribute<TestFixtureForAttribute>().ClassToTest;
            var fixture = classToTest.GetCustomAttribute<TestedByAttribute>().Fixture;
            type.Should().Be(fixture);
        }
    }
}