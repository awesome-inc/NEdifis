using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    public static class ConventionsExtensions
    {
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
                var message = string.Join("\r\n", Enumerable.Range(0, failures.Count).Select(i => $"  {i+1}. {failures[i].Message}"));
                throw new AssertionException($"Some conventions were broken:\r\n{message}", 
                    new AggregateException(failures));
            }
        }

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