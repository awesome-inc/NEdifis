using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(ExcludeFromCodeCoverageClassHasBecauseAttribute_Should))]
    public class ExcludeFromCodeCoverageClassHasBecauseAttribute
        : IVerifyConvention
    {
        public string HintOnFail
        {
            get
            {
                return "~a class with an ExcludeFromCodeCoverage attribute requires a Because attribute~";
            }
        }

        public bool FulfilsConvention(Type t)
        {
            var efccAttribute = t.GetCustomAttribute<ExcludeFromCodeCoverageAttribute>(false);
            if (efccAttribute == null) return true;

            var becauseAttribute = t.GetCustomAttribute<BecauseAttribute>();
            if (becauseAttribute == null) return false;

            if (string.IsNullOrWhiteSpace(becauseAttribute.Reason)) return false;

            return true;
        }
    }
}
