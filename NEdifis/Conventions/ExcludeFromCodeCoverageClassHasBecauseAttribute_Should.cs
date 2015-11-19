using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ExcludeFromCodeCoverageClassHasBecauseAttribute))]
    // ReSharper disable once InconsistentNaming
    internal class ExcludeFromCodeCoverageClassHasBecauseAttribute_Should
    {
        // ReSharper disable once InconsistentNaming
        [ExcludeFromCodeCoverage]
        private class Excluded_From_Code_Without_Because { }

        [ExcludeFromCodeCoverage]
        [Because("this is a test")]
        // ReSharper disable once InconsistentNaming
        private class Excluded_From_Code_With_Because { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new ExcludeFromCodeCoverageClassHasBecauseAttribute();
            sut.Should().NotBeNull();
            sut.HintOnFail.Should().NotBeNullOrWhiteSpace();

            sut.FulfilsConvention(typeof(Excluded_From_Code_Without_Because)).Should().Be(false);
            sut.FulfilsConvention(typeof(Excluded_From_Code_With_Because)).Should().Be(true);
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new ExcludeFromCodeCoverageClassHasBecauseAttribute();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}