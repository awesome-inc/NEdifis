using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    public abstract class VerifyAttributesAndConventionsBase
    {
        private readonly Assembly _assembly;

        protected VerifyAttributesAndConventionsBase()
        {
            _assembly = GetType().Assembly;
        }

        #region data sources

        public object[] AllClasses()
        {
            // ReSharper disable once CoVariantArrayConversion
            return _assembly
                .GetTypes()
                .Where(t => !typeof(VerifyAttributesAndConventionsBase).IsAssignableFrom(t)) // we exclude convention tests and derivants
                .Where(t=> !(t.IsNested && !t.IsPublic) && t.IsClass) // and we exclude interfaces!
                .Select(t => new object[] { t })
                .ToArray();
        }

        public object[] ShouldClasses()
        {
            // ReSharper disable once CoVariantArrayConversion
            return _assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("_Should"))
                .Select(t => new object[] { t })
                .ToArray();
        }

        public object[] TestFixtureForClasses()
        {
            // ReSharper disable once CoVariantArrayConversion
            return _assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<TestFixtureForAttribute>(false) != null)
                .Select(t => new object[] { t })
                .ToArray();
        }

        public object[] ExcludeFromCodeCoverageClasses()
        {
            // ReSharper disable once CoVariantArrayConversion
            return _assembly
                .GetTypes()
                .Where(t => t.GetCustomAttribute<ExcludeFromCodeCoverageAttribute>(false) != null)
                .Select(t => new object[] { t })
                .ToArray();
        }

        #endregion

        public void ExcludeFromCodeCoverage_Need_A_Because(Type cls)
        {
            var efccAttribute = cls.GetCustomAttributes<ExcludeFromCodeCoverageAttribute>();
            if (efccAttribute == null) return;

            var becauseAttribute = cls.GetCustomAttribute<BecauseAttribute>();
            becauseAttribute.Should().NotBeNull(because: string.Format("{0} is excluded from code coverage but has no 'Because' attribute", cls.FullName));
            becauseAttribute.Reason.Should().NotBeNullOrWhiteSpace(because: string.Format("{0} is excluded from code coverage with an empty 'Because' attribute (no reason)", cls.FullName));
        }

        public void AllClasses_Need_A_Test(Type cls)
        {
            if (cls.Name.EndsWith("_Should")) return; // because we dont test this here
            if (cls.GetCustomAttribute<ExcludeFromCodeCoverageAttribute>() != null) return;

            var testedByAttribute = cls.GetCustomAttribute<TestedByAttribute>();
            if (testedByAttribute == null)
                Assert.Fail("Class {0} has not tested by class", cls.FullName);
        }

        public void TestFixtureFor_Have_A_Symetric_TestedBy_Class(Type testFixtureForClass)
        {
            var tffAttribute = testFixtureForClass.GetCustomAttribute<TestFixtureForAttribute>();
            if (tffAttribute == null) return;

            var tbAttribute = tffAttribute.TestClass.GetCustomAttribute<TestedByAttribute>();
            tbAttribute.Should().NotBeNull(because: string.Format("The class {0} has a TestFixtureAttribute, but the tested class {1} has not", testFixtureForClass, tffAttribute.TestClass.FullName));

            // make sure the reference goes the other way
            tbAttribute.TestClass.Should().Be(testFixtureForClass, because: string.Format("Class {0} should reference {1}", tffAttribute.TestClass.Name, testFixtureForClass));

            // now lets see if the naming fits
            var classNameWithoutGeneric = tffAttribute.TestClass.Name.Split('`')[0];
            testFixtureForClass.Name.Should().Be(classNameWithoutGeneric + "_Should");
        }

        public void TestFixtureFor_End_With_Should(Type testFixtureForClass)
        {

            var isTestFixtureForAndNotShould = testFixtureForClass.Name.EndsWith("_Should") &&
                                               testFixtureForClass.GetCustomAttribute<TestFixtureForAttribute>(false) == null;

            isTestFixtureForAndNotShould.Should().BeFalse(
                because: string.Format("{0} ends with '_Should' but does not have a 'TestFixtureFor' attribute", testFixtureForClass.FullName));
        }

        public void Should_Classes_Need_To_Be_A_TestFixtureFor(Type shouldClass)
        {

            var isShouldAndNotTestFixtureFor = shouldClass.Name.EndsWith("_Should") &&
                                               shouldClass.GetCustomAttribute<TestFixtureForAttribute>(false) == null;

            isShouldAndNotTestFixtureFor.Should().BeFalse(
                because: string.Format("{0} ends with '_Should' but does not have a 'TestFixtureFor' attribute", shouldClass.FullName));
        }

        public void Should_Classes_Must_Not_Be_Public(Type shouldClass)
        {
            var isShouldAndPublic = shouldClass.Name.EndsWith("_Should") &&
                                    shouldClass.IsPublic;

            isShouldAndPublic.Should().BeFalse(
                because: string.Format("{0} has a 'TestFixtureFor' attribute and should be internal/private", shouldClass.FullName));
        }
    }
}