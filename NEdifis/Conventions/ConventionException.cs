using System;

namespace NEdifis.Conventions
{
    /// <summary>
    /// An exception to be thrown when a convention (<see cref="IVerifyConvention"/>) is broken.
    /// </summary>
    public class ConventionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionException"/>.
        /// </summary>
        /// <param name="convention">The convention that was broken</param>
        /// <param name="checkedType">The type that brokes the specified convention</param>
        /// <param name="innerException">An inner exception</param>
        public ConventionException(IVerifyConvention convention, Type checkedType, Exception innerException)
            : base($"Type '{checkedType}' breaks convention '{convention.GetType().Name}': {innerException.Message}", innerException)
        {
        }
    }
}