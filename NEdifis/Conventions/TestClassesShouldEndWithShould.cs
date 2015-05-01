using System;
using System.Reflection;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(TestClassesShouldEndWithShould_Should))]
    class TestClassesShouldEndWithShould : IVerifyConvention
    {
        public string HintOnFail
        {
            get { return "Test classes should end with '_Should' and should have a 'TestFixtureFor' attribute"; }
        }

        public bool FulfilsConvention(Type t)
        {
            if (t.Name.EndsWith("_Should"))
                return t.GetCustomAttribute<TestFixtureForAttribute>(true) != null;

            if (t.GetCustomAttribute<TestFixtureForAttribute>(true) != null)
                return t.Name.EndsWith("_Should");

            return true;
        }
    }
}