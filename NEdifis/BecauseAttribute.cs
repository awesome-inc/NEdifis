using System;
using System.ComponentModel;

namespace NEdifis
{
    /// <summary>
    /// Attribute to attach a reason to a method especially if you use attributes
    /// which dont support a reson.
    /// </summary>
    public class BecauseAttribute : Attribute
    {
        public BecauseAttribute(string reason)
        {
            Reason = reason;
        }

        [Browsable(false)]
        public string Reason { get; private set; }
    }
}