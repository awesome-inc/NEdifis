using System;

namespace NEdifis.Attributes
{
    /// <summary>
    /// Attribute to exclude a class from convention checkes.
    /// </summary>
    public class ExcludeFromConventionsAttribute : Attribute
    {
        /// <summary>
        /// A reason explaining why to exclude the attributed class from convention checks.
        /// </summary>
        public string Because { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcludeFromConventionsAttribute"/>.
        /// </summary>
        /// <param name="because">A reason explaining why to exclude the attributed class from convention checks</param>
        /// <exception cref="ArgumentNullException">Thrown, when the specified reason is null</exception>
        /// <exception cref="ArgumentException">Thrown, when the specified reason is empty or whitespace</exception>
        public ExcludeFromConventionsAttribute(string because)
        {
            if (because == null) throw new ArgumentNullException(nameof(because));
            if (string.IsNullOrWhiteSpace(because)) throw new ArgumentException("Parameter must not be null, empty or whitespace", nameof(because));
            Because = because;
        }
    }
}