using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;

namespace NEdifis.Data
{
    /// <summary>
    /// Randomizes values and test data
    /// </summary>
    public static class Randomized
    {
        private static readonly Randomizer Randomizer = new Randomizer(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// gets a random value from a list of values
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T Random<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var sourceArray = source as T[] ?? source.ToArray();
            if (!sourceArray.Any()) throw new ArgumentOutOfRangeException(nameof(source), "source does not contain elements");

            var index = Randomizer.Next(0, sourceArray.Length);
            return sourceArray.Skip(index).First();
        }

        /// <summary>
        /// ramdonizes values of an enumeration
        /// </summary>
        /// <param name="sourceStatus"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Random(this Type sourceStatus)
        {
            var sourceArray = Enum.GetNames(sourceStatus);
            if (!sourceArray.Any()) throw new ArgumentOutOfRangeException(nameof(sourceStatus), "source does not contain elements");

            var index = Randomizer.Next(0, sourceArray.Length);
            return sourceArray.Skip(index).First();
        }
    }
}