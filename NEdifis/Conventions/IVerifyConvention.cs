using System;

namespace NEdifis.Conventions
{
    /// <summary>
    /// A code convention interface
    /// </summary>
    public interface IVerifyConvention
    {
        /// <summary>
        /// A filter to select types where this convention can be applied.
        /// </summary>
        Func<Type, bool> Filter { get; } 
        
        /// <summary>
        /// Asserts the convention on the specified type.
        /// </summary>
        /// <param name="type">The type to check</param>
        void Verify(Type type);
    }
}