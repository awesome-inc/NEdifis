using System;

namespace NEdifis.Attributes
{
    public class ExcludeFromConventionsAttribute : Attribute
    {
        public string Because { get; private set; }

        public ExcludeFromConventionsAttribute(string because)
        {
            Because = because;
        }
    }
}