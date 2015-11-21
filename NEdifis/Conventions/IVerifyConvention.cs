using System;

namespace NEdifis.Conventions
{
    public interface IVerifyConvention
    {
        Func<Type, bool> Filter { get; } 
        void Verify(Type type);
    }
}