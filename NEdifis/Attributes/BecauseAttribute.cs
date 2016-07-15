using System;

namespace NEdifis.Attributes
{
    /// <summary>
    /// Attribute to attach a reason to a method, especially if you use attributes
    /// which dont support a reson like ExcludeFromCodeCoverageAttribute
    /// </summary>
    public class BecauseAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BecauseAttribute"/>.
        /// </summary>
        /// <param name="reason">A reason explaining why to exclude the attributed class from convention checks</param>
        /// <exception cref="ArgumentNullException">Thrown, when the specified reason is null</exception>
        /// <exception cref="ArgumentException">Thrown, when the specified reason is empty or whitespace</exception>
        public BecauseAttribute(string reason)
        {
            if (reason == null) throw new ArgumentNullException(nameof(reason));
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Parameter must not be null, empty or whitespace", nameof(reason));

            Reason = reason;
        }

        /// <summary>
        /// A reason explaining why to exclude the attributed class from convention checks.
        /// </summary>
        public string Reason { get; private set; }
    }
}