using System;

namespace NEdifis.Conventions
{
    public interface IVerifyConvention
    {
        string HintOnFail { get; }
        bool FulfilsConvention(Type t);
    }
}