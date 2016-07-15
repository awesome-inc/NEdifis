using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    /// <summary>
    /// Extension methods for <see cref="IVerifyConvention"/>
    /// </summary>
    public static class ConventionsExtensions
    {
        /// <summary>
        /// Applies the specified <see paramref="conventions"/> to the specified <see paramref="typeTotest"/>.
        /// </summary>
        /// <param name="conventions">The conventions to apply</param>
        /// <param name="typeToTest">The type to test</param>
        /// <exception cref="AssertionException">Thrown when one or more conventions are broken</exception>
        public static void Check(this IEnumerable<IVerifyConvention> conventions, Type typeToTest)
        {
            var failures = new List<Exception>();

            foreach (var convention in conventions)
            {
                try { convention.Check(typeToTest); }
                catch (Exception ex) { failures.Add(ex); }
            }

            if (failures.Any())
            {
                var message = string.Join(Environment.NewLine, Enumerable.Range(0, failures.Count).Select(i => $"  {i+1}. {failures[i].Message}"));
                throw new AssertionException($"Some conventions were broken:{Environment.NewLine}{message}", 
                    new AggregateException(failures));
            }
        }

        /// <summary>
        /// Applies the specified <see paramref="convention"/> to the specified <see paramref="typeTotest"/>.
        /// </summary>
        /// <param name="convention">The conventions to check</param>
        /// <param name="typeToTest">The type to test</param>
        /// <exception cref="ConventionException">Thrown when the convention is broken</exception>
        public static void Check(this IVerifyConvention convention, Type typeToTest)
        {
            try
            {
                var filter = convention.Filter;
                if (filter == null || filter(typeToTest))
                    convention.Verify(typeToTest);
            }
            catch (ConventionException) { throw; }
            catch (Exception ex) 
            {
                throw new ConventionException(convention, typeToTest, ex);
            }
        }
    }
}