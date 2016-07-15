using System;

namespace NEdifis.Attributes
{
    /// <summary>
    /// Attribute to annotate a test fixture or test method with an issue.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class IssueAttribute : Attribute
    {
        /// <summary>
        /// Issue id, e.g. for GitHub Issues this might be the resource url (http://github/myrepo/issues/42).
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// An optional issue title, e.g. for GitHub issues this might be the summary or description.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssueAttribute"/>.
        /// </summary>
        /// <param name="id">The issue id</param>
        /// <exception cref="ArgumentNullException">Thrown, when the specified id is null</exception>
        /// <exception cref="ArgumentException">Thrown, when the specified id is empty or whitespace</exception>
        public IssueAttribute(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Parameter must not be null, empty or whitespace", nameof(id));
            Id = id;
        }
    }
}
