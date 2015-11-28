using System;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    public class TestClassesShouldMatchClassToTest : IVerifyConvention
    {
        public string TestClassSuffix { get; set; } = "_Should";
        public Func<Type, bool> Filter { get; }

        public TestClassesShouldMatchClassToTest()
        {
            Filter = type => type.Name.EndsWith(TestClassSuffix) || type.IsDecoratedWith<TestFixtureForAttribute>();
        }

        public void Verify(Type type)
        {
            type.Name.Should().EndWith(TestClassSuffix);
            type.Should().BeDecoratedWith<TestFixtureForAttribute>();

            var testName = type.FullName;
            var name = type.GetCustomAttribute<TestFixtureForAttribute>().ClassToTest.FullName;
            var p = name.IndexOf('`'); // cutof generics
            if (p > 0) name = name.Substring(0, p);
            testName.Should().Be(name + TestClassSuffix);
        }
    }
}