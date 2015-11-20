using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    internal class NedifisConventions : ConventionBase
    {
        private static IEnumerable<Type> TypesToTest { get; } = ClassesToTestFor<NedifisConventions>();

        public NedifisConventions()
        {
            Conventions.AddRange(ConventionsFor<NedifisConventions>());
        }

        [Test, TestCaseSource(nameof(TypesToTest))]
        public void Check(Type typeToTest)
        {
            Conventions.Check(typeToTest);
        }

        // 2nd way for conventions using 2 ValueSource
        //private static IEnumerable<IVerifyConvention> ConventionsToCheck { get; } = ConventionsFor<NedifisConventions>();
        //[Test]
        //public void Check(
        //    [ValueSource(nameof(TypesToTest))]Type typeToTest,
        //    [ValueSource(nameof(ConventionsToCheck))]IVerifyConvention convention)
        //{
        //    convention.Check(typeToTest);
        //}
    }
}