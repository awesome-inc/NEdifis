using System;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixture]
    public class VerifyAllAttributesAndConventions : VerifyAttributesAndConventionsBase
    {
        [Test]
        [TestCaseSource("ExcludeFromCodeCoverageClasses")]
        public void ExcludeFromCodeCoverageClasses_Need_A_Because(Type shouldClass)
        {
            base.ExcludeFromCodeCoverage_Need_A_Because(shouldClass);
        }

        [Test]
        [TestCaseSource("AllClasses")]
        public void AllClassesNeedTestedBy(Type shouldClass)
        {
            base.AllClasses_Need_A_Test(shouldClass);
        }

        [Test]
        [TestCaseSource("TestFixtureForClasses")]
        public void TestFixtureFor_Have_A_TestedBy(Type shouldClass)
        {
            base.TestFixtureFor_End_With_Should(shouldClass);
            base.TestFixtureFor_Have_A_Symetric_TestedBy_Class(shouldClass);
        }

        [Test]
        [TestCaseSource("ShouldClasses")]
        public void VerifyShouldClasses(Type shouldClass)
        {
            base.Should_Classes_Must_Not_Be_Public(shouldClass);
            base.Should_Classes_Need_To_Be_A_TestFixtureFor(shouldClass);
        }
    }
}