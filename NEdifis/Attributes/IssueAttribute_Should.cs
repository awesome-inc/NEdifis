using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(IssueAttribute))]
    [Issue("#42", Title = "Create an attribute to assign a ticket id")]
    // ReSharper disable once InconsistentNaming
    internal class IssueAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            // ReSharper disable ObjectCreationAsStatement
            0.Invoking(x => new IssueAttribute(null)).Should().Throw<ArgumentException>();
            1.Invoking(x => new IssueAttribute("\t\r\n")).Should().Throw<ArgumentException>();
            // ReSharper restore ObjectCreationAsStatement

            var sut = new IssueAttribute("#42");

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<Attribute>();
        }

        [Test]
        [Issue("13", Title = "a test should resolve or be related to multiple tickets")]
        public void Allow_Multiple()
        {
            var att = typeof(IssueAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            att.Should().NotBeNull();
            att.AllowMultiple = true;
            att.ValidOn.Should().Be(AttributeTargets.All);
        }

        [Test]
        public void Have_Id_and_Title()
        {
            var sut = new IssueAttribute("#23") { Title = "foo bar" };
            sut.Id.Should().Be("#23");
            sut.Title.Should().Be("foo bar");
        }
    }
}
