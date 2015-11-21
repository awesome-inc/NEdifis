using System;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(ConventionException_Should))]
    public class ConventionException : Exception
    {
        public ConventionException(IVerifyConvention convention, Type checkedType, Exception innerException)
            : base($"Type '{checkedType}' breaks convention '{convention.GetType().Name}': {innerException.Message}", innerException)
        {
        }
    }
}