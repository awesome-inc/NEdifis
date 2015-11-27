using System;

namespace NEdifis.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class IssueAttribute : Attribute
    {
        public string Id { get; private set; }
        public string Title { get; set; }

        public IssueAttribute(string id)
        {
            Id = id;
        }
    }
}
