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
    /// <summary>
    /// A convention class ready to serve as a collection of your conventions.
    /// </summary>
    [TestFixture]
    public class ConventionBase
    {
        /// <summary>
        /// The conventions to apply on the selected types
        /// </summary>
        public List<IVerifyConvention> Conventions { get; } = new List<IVerifyConvention>();

        /// <summary>
        /// A helper function to select the types for convention checks.
        /// </summary>
        /// <param name="pattern">An optional type selector</param>
        /// <typeparam name="TFixture">A fixture type that is used to select the assembly to scan for types</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> ClassesToTestFor<TFixture>(Func<Type, bool> pattern = null) 
        {
            return typeof(TFixture).Assembly.GetTypes()
                .Where(pattern ?? DefaultClassPattern);
        }

        /// <summary>
        /// A helper function to select conventions dynamically from an assembly.
        /// </summary>
        /// <param name="pattern">An optional type selector</param>
        /// <typeparam name="TConvention">A convention type that is used to select the assembly to scan for conventions</typeparam>
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
