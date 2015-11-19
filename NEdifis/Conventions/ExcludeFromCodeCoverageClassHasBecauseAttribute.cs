using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(ExcludeFromCodeCoverageClassHasBecauseAttribute_Should))]
    public class ExcludeFromCodeCoverageClassHasBecauseAttribute
        : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; } =
            type => type.GetCustomAttribute<ExcludeFromCodeCoverageAttribute>(false) != null;

        public void Verify(Type type)
        {
            var becauseAttribute = type.GetCustomAttribute<BecauseAttribute>();
            becauseAttribute.Should().NotBeNull();
            becauseAttribute.Reason.Should().NotBeNullOrWhiteSpace();
        }
    }
}
