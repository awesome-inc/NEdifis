using System;
using FluentAssertions;
using NEdifis.Attributes;

namespace NEdifis.Conventions
{
    [TestedBy(typeof(AllClassesNeedATest_Should))]
    public class AllClassesNeedATest : IVerifyConvention
    {
        public Func<Type, bool> Filter { get; set; } = type => type.IsClass && !type.Name.EndsWith("_Should");

        public void Verify(Type type)
        {
            type.Should().BeDecoratedWith<TestedByAttribute>();
        }
    }
}
