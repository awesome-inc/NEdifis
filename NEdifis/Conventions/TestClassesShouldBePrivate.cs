using System;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(TestClassesShouldBePrivate_Should))]
    public class TestClassesShouldBePrivate : IVerifyConvention
    {
        public string HintOnFail
        {
            get { return "~all test classes should be private~"; }
        }

        public bool FulfilsConvention(Type t)
        {
            if (!t.Name.EndsWith("_Should")) return true;

            return !t.IsPublic;
        }
    }
}