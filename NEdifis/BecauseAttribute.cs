using System;
using System.ComponentModel;

namespace NEdifis
{
    /// <summary>
    /// Attribute to attach a reason to a method especially if you use attributes
    /// which dont support a reson.
    /// </summary>
    [TestedBy(typeof(BecauseAttribute_Should))]
    public class BecauseAttribute : Attribute
    {
        public BecauseAttribute(string reason)
        {
            if (reason == null) throw new ArgumentNullException("reason");
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("reason cannot be null or whitespace", "reason");

            Reason = reason;
        }

        [Browsable(false)]
        public string Reason { get; private set; }
    }
}