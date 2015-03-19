using NUnit.Framework;

namespace NEdifis.Demo
{
    [Explicit("this test has wanted fails")]
    [Category("ExcludedTest")]
    public class VerifyConventions
        : VerifyAllAttributesAndConventions
    {
    }
}