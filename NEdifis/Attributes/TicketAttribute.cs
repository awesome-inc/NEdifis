using System;

namespace NEdifis.Attributes
{
    [TestedBy(typeof(TicketAttribute_Should))]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TicketAttribute : Attribute
    {
        public string Id { get; private set; }
        public string Title { get; set; }

        public TicketAttribute(string id)
        {
        }
    }
}
