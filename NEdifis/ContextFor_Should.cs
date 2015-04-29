using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace NEdifis
{
    [TestFixtureFor(typeof(ContextFor<>))]
    public class ContextFor_Should
    {
        [TestFixture]
        public class ContextFor_Cannot
        {
            [Test]
            public void Handle_Class_With_Nested_Constructor_Parameter()
            {
                new Action(() => new ContextFor<Class_With_Nested_Constructor_Parameter>())
                    .Invoking(a => a.Invoke())
                    .ShouldThrow<Exception>();

                // because NSubstitute does not support class with constructor parameter
                new Action(() => Substitute.For<Class_With_One_Constructor_Parameter>())
                    .Invoking(a => a.Invoke())
                    .ShouldThrow<Exception>();

                // but it support parameterless constructors
                Substitute.For<Class_Without_Constructor>();

                // as long as they are public
                new Action(() => Substitute.For<Class_With_One_Empty_Constructor>())
                    .Invoking(a => a.Invoke())
                    .ShouldThrow<Exception>();

            }
        }

        #region test classes

        private class Class_Without_Constructor { }

        private class Class_With_One_Empty_Constructor
        {
            // ReSharper disable once EmptyConstructor
            public Class_With_One_Empty_Constructor() { }
        }

        private class Class_With_One_Constructor_Parameter
        {
            public Class_With_One_Constructor_Parameter(IList<string> param1) { }
        }

        [ExcludeFromCodeCoverage]
        [Because("this is a test")]
        private class Class_With_Nested_Constructor_Parameter
        {
            private Class_With_One_Constructor_Parameter Param1 { get; set; }

            public Class_With_Nested_Constructor_Parameter(Class_With_One_Constructor_Parameter param1)
            {
                Param1 = param1;
            }
        }

        private class Class_With_Two_Constructor_Parameter
        {
            public Class_With_Two_Constructor_Parameter(IList<string> param1, ICloneable cloneable) { }
        }

        private class Class_With_Optional_Constructor_Parameter
        {
            public Class_With_Optional_Constructor_Parameter(IList<string> param1, ICloneable cloneable = null) { }
        }

        private class Class_With_Two_Similar_Constructor_Parameter
        {
            public IList<string> Param1 { get; private set; }
            public IList<string> Param2 { get; private set; }

            public Class_With_Two_Similar_Constructor_Parameter(IList<string> param1, IList<string> param2)
            {
                Param1 = param1;
                Param2 = param2;
            }
        }

        private class Class_With_Two_Constructors
        {
            public IList<string> Param1 { get; set; }
            public IList<string> Param2 { get; set; }

            public Class_With_Two_Constructors(IList<string> param1)
            {
                Param1 = param1;
                Param2 = null;
            }

            public Class_With_Two_Constructors(IList<string> param1, IList<string> param2)
            {
                Param1 = param1;
                Param2 = param2;
            }
        }

        #endregion

        [Test]
        public void Handle_Class_Without_Constructor()
        {
            var ctx = new ContextFor<Class_Without_Constructor>();
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void Throw_Exception_On_Access_Non_Existing_Parameter()
        {
            var ctx = new ContextFor<Class_With_One_Constructor_Parameter>();

            // on generic
            new Action(() => ctx.For<IFormattable>()).ShouldThrow<ArgumentException>();

            // on named parameter
            new Action(() => ctx.For<IList<string>>("this_does_not_exist")).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Throw_Exception_On_Wrong_Expectation_On_Parameter_Type()
        {
            var ctx = new ContextFor<Class_With_One_Constructor_Parameter>();

            // wrong parameter type
            new Action(() => ctx.For<IFormattable>("param1")).ShouldThrow<InvalidCastException>();
        }

        [Test]
        public void Handle_Class_With_One_Empty_Constructor()
        {
            var ctx = new ContextFor<Class_With_One_Empty_Constructor>();
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void Handle_Class_With_One_Constructor_Parameter()
        {
            var ctx = new ContextFor<Class_With_One_Constructor_Parameter>();
            ctx.For<IList<string>>().Should().NotBeNull();
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void Handle_Class_With_Two_Constructor_Parameter()
        {
            var ctx = new ContextFor<Class_With_Two_Constructor_Parameter>();

            var param1 = ctx.For<IList<string>>();
            var param2 = ctx.For<ICloneable>();
            param1.Should().NotBeNull();
            param2.Should().NotBeNull();

            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void Handle_Class_With_Optional_Constructor_Parameter_And_No_Substitute_For_Parameter()
        {
            var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>(substituteOptionalParameter: false);

            var param1 = ctx.For<IList<string>>();
            var param2 = ctx.For<ICloneable>();
            param1.Should().NotBeNull();
            param2.Should().BeNull();

            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void Handle_Class_With_Optional_Constructor_Parameter_And_Substitute_For_Parameter()
        {
            var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>(substituteOptionalParameter: true);

            var param1 = ctx.For<IList<string>>();
            var param2 = ctx.For<ICloneable>();
            param1.Should().NotBeNull();
            param2.Should().NotBeNull();

            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
        }

        [Test]
        public void Handle_Class_With_Two_Similar_Constructor_Parameter()
        {
            var ctx = new ContextFor<Class_With_Two_Similar_Constructor_Parameter>();

            // it always should return the first parameter
            var param1 = ctx.For<IList<string>>();
            var param2 = ctx.For<IList<string>>();
            param1.Should().NotBeNull();
            param2.Should().NotBeNull();
            param2.Equals(param1).Should().BeTrue();

            // now we get it explicitly
            var param1a = ctx.For<IList<string>>("param1");
            var param2a = ctx.For<IList<string>>("param2");
            param1a.Should().NotBeNull();
            param2a.Should().NotBeNull();
            param2a.Equals(param1a).Should().BeFalse();

            // lets see if the params gut injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Param1.Should().Equal(param1a);
            sut.Param2.Should().Equal(param2a);
        }

        [Test]
        public void Handle_Class_With_Two_Constructors()
        {
            var ctx = new ContextFor<Class_With_Two_Constructors>();

            // it always should return the first parameter
            var param1 = ctx.For<IList<string>>();
            var param2 = ctx.For<IList<string>>();
            param1.Should().NotBeNull();
            param2.Should().NotBeNull();
            param2.Equals(param1).Should().BeTrue();

            // now we get it explicitly
            var param1a = ctx.For<IList<string>>("param1");
            var param2a = ctx.For<IList<string>>("param2");
            param1a.Should().NotBeNull();
            param2a.Should().NotBeNull();
            param2a.Equals(param1a).Should().BeFalse();

            // lets see if the params gut injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Param1.Should().Equal(param1a);
            sut.Param2.Should().Equal(param2a);
        }

        [Test]
        public void Handle_Class_With_Two_Constructors_And_Use_Types()
        {
            var ctx = new ContextFor<Class_With_Two_Constructors>(typeof(IList<string>));

            // it always should return the first parameter
            var param1 = ctx.For<IList<string>>();
            param1.Should().NotBeNull();

            // lets see if the params gut injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Param1.Should().NotBeNull();
            sut.Param2.Should().BeNull();
        }

        [Test]
        public void Handle_Class_With_Two_Constructors_But_Call_Explicit()
        {
            var info = typeof(Class_With_Two_Constructors).GetConstructor(new[] { typeof(IList<string>) });
            var ctx = new ContextFor<Class_With_Two_Constructors>(info);

            // it always should return the first parameter
            var param1 = ctx.For<IList<string>>();
            var param2 = ctx.For<IList<string>>();
            param1.Should().NotBeNull();
            param2.Should().NotBeNull();
            param2.Equals(param1).Should().BeTrue();

            // now we get it explicitly
            var param1a = ctx.For<IList<string>>("param1");
            ctx.Invoking(c => c.For<IList<string>>("param2")).ShouldThrow<ArgumentException>();
            param1a.Should().NotBeNull();

            // lets see if the params gut injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Param1.Should().Equal(param1a);
            sut.Param2.Should().BeNull();
        }

    }
}