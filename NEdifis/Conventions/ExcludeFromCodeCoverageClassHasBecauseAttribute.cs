using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    /// <summary>
    /// A convention that checks that a class excluded from code coverage (<see cref="ExcludeFromCodeCoverageAttribute"/>) 
    /// is also attributed with <see cref="BecauseAttribute"/> to explain why it's excluded.
    /// This assumes that by default all classes should be covered by tests.
    /// </summary>
    public class ExcludeFromCodeCoverageClassHasBecauseAttribute
        : IVerifyConvention
    {
        /// <summary>
        /// A filter to select types where this convention can be applied.
        /// </summary>
        public Func<Type, bool> Filter { get; } =
            type => type.GetCustomAttribute<ExcludeFromCodeCoverageAttribute>(false) != null;

        /// <summary>
        /// Asserts the convention on the specified type.
        /// </summary>
        /// <param name="type">The type to check</param>
        public void Verify(Type type)
        {
            var becauseAttribute = type.GetCustomAttribute<BecauseAttribute>();
            becauseAttribute.Should().NotBeNull();
            becauseAttribute.Reason.Should().NotBeNullOrWhiteSpace();
        }
    }
}
