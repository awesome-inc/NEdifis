using System.Linq;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Data
{
    [TestFixtureFor(typeof(Randomized))]
    // ReSharper disable InconsistentNaming
    internal class Randomized_Should
    {
        [Test]
        public void Return_A_Random_Item_From_List()
        {
            var items = Enumerable.Range(1, 100).ToList();
            items.Random().Should().BeInRange(1, 100);
        }
    }
}