using System;

namespace NEdifis.Attributes
{
    [TestedBy(typeof(TicketAttribute_Should))]
    public class TicketAttribute : Attribute
    {
        public string Reference { get; private set; }
        public int? Id { get; private set; }

        public TicketAttribute(string reference) : this(null, reference) { }

        public TicketAttribute(int id) : this(id, null) { }

        public TicketAttribute(int? id, string reference)
        {
            Id = id;
            Reference = reference;
        }
    }
}
