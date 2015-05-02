using System;
using System.Reflection;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(AllClassesNeedATest_Should))]
    public class AllClassesNeedATest : IVerifyConvention
    {
        public string HintOnFail
        {
            get
            {
                return "~all classes which are not a '_Should' should have a 'TestedBy' attribute~";
            }
        }

        public bool FulfilsConvention(Type cls)
        {
            if (cls.Name.EndsWith("_Should")) return true; // because we dont test this here

            var testedByAttribute = cls.GetCustomAttribute<TestedByAttribute>();
            if (testedByAttribute == null) return false;

            return true;
        }
    }
}
