using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixture]
    public class ConventionBase
    {
        public List<IVerifyConvention> Conventions { get; } = new List<IVerifyConvention>();

        public static IEnumerable<Type> ClassesToTestFor<TFixture>(Func<Type, bool> pattern = null) 
        {
            return typeof(TFixture).Assembly.GetTypes()
                .Where(pattern ?? DefaultClassPattern);
        }

        public static IEnumerable<IVerifyConvention> ConventionsFor<TConvention>(Func<Type, bool> pattern = null)
        {
            return typeof(TConvention).Assembly.GetTypes()
                .Where(pattern ?? DefaultConventionPattern)
                .Select(t => (IVerifyConvention)Activator.CreateInstance(t)); // <-- how to impose new() constraint ?
        }

        private static readonly Func<Type, bool> DefaultClassPattern = t => 
            !typeof(ConventionBase).IsAssignableFrom(t) &&
                !t.GetCustomAttributes<ExcludeFromConventionsAttribute>(true).Any() &&
                !t.IsInterface &&
                !IsGenerated(t) &&
                !t.IsNested;

        private static readonly Func<Type, bool> DefaultConventionPattern = t =>
            typeof(IVerifyConvention).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !IsGenerated(t) &&
                !t.IsNested;

        private static bool IsGenerated(MemberInfo type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes<CompilerGeneratedAttribute>(false).Any();
            var hasGeneratedCodeAttribute = type.GetCustomAttributes<GeneratedCodeAttribute>(false).Any();
            return hasCompilerGeneratedAttribute || hasGeneratedCodeAttribute;
        }
    }
}
