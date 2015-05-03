using System;

namespace NEdifis.Attributes
{
    [TestedBy(typeof(ExcludeFromConventionsAttribute_Should))]
    public class ExcludeFromConventionsAttribute : Attribute
    {
        public string Because { get; private set; }

        public ExcludeFromConventionsAttribute(string because)
        {
            Because = because;
        }
    }
}