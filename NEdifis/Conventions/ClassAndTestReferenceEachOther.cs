using System;
using System.Reflection;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(ClassAndTestReferenceEachOther_Should))]
    class ClassAndTestReferenceEachOther : IVerifyConvention
    {
        public string HintOnFail
        {
            get { return "~A class and the test class must reference each other using 'TestedBy' and 'TestFixtureFor' attribute~"; }
        }

        public bool FulfilsConvention(Type t)
        {
            if (t.Name.EndsWith("_Should"))
                return VerifyTestClass(t);

            return VerifyClass(t);
        }

        public bool VerifyClass(MemberInfo t)
        {
            var tbAttribute = t.GetCustomAttribute<TestedByAttribute>();
            if (tbAttribute == null) return false;

            var tffAttribute = tbAttribute.TestClass.GetCustomAttribute<TestFixtureForAttribute>();
            if (tffAttribute == null) return false;

            return t == tffAttribute.TestClass;
        }

        private static bool VerifyTestClass(MemberInfo t)
        {
            var tffAttribute = t.GetCustomAttribute<TestFixtureForAttribute>();
            if (tffAttribute == null) return false;

            var tbAttribute = tffAttribute.TestClass.GetCustomAttribute<TestedByAttribute>();
            if (tbAttribute == null) return false;

            return t == tbAttribute.TestClass;
        }
    }
}