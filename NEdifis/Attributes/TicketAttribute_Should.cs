using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(TicketAttribute))]
    [Ticket(4, Title = "Create an attribute to assign a ticket id")]
    // ReSharper disable once InconsistentNaming
    class TicketAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var ctx = new ContextFor<TicketAttribute>();
            var sut = ctx.BuildSut();

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<Attribute>();
        }

        [Test]
        [Ticket(13, Title = "a test should resolve or be related to multiple tickets")]
        public void Allow_Multiple()
        {
            var att = typeof(TicketAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            att.Should().NotBeNull();
            att.AllowMultiple = true;
            att.ValidOn.Should().Be(AttributeTargets.All);
        }

        [Test]
        public void Have_Id()
        {
            var sut = new TicketAttribute(23);
            sut.Id.Should().Be(23);
            sut.Reference.Should().BeNull();
        }

        [Test]
        public void Have__a_Title()
        {
            var sut = new TicketAttribute(23) { Title = "foo bar" };

            sut.Id.Should().Be(23);
            sut.Reference.Should().BeNull();
            sut.Title.Should().Be("foo bar");
        }

        [Test]
        public void Have_Reference()
        {
            var sut = new TicketAttribute("#42");
            sut.Reference.Should().Be("#42");
            sut.Id.Should().NotHaveValue();
        }

        [Test]
        public void Have_Id_And_Reference()
        {
            var sut = new TicketAttribute(23, "#42");
            sut.Reference.Should().Be("#42");
            sut.Id.Should().Be(23);
        }
    }
}