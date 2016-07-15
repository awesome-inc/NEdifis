using System;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Common;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    /// <summary>
    /// The 'fixture names should match the classe names they test'-convention.
    /// </summary>
    public class TestClassesShouldMatchClassToTest : IVerifyConvention
    {
        /// <summary>
        /// The default suffix for fuxtures. By default this is '_Should';
        /// </summary>
        public string TestClassSuffix { get; set; } = "_Should";

        /// <summary>
        /// A filter to select types where this convention can be applied.
        /// </summary>
        public Func<Type, bool> Filter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClassesShouldMatchClassToTest"/>.
        /// </summary>
        public TestClassesShouldMatchClassToTest()
        {
            Filter = type => type.Name.EndsWith(TestClassSuffix) || type.IsDecoratedWith<TestFixtureForAttribute>();
        }

        /// <summary>
        /// Asserts the convention on the specified type.
        /// </summary>
        /// <param name="type">The type to check</param>
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