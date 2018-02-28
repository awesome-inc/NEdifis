using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace NEdifis
{
    [TestFixtureFor(typeof(ContextFor<>))]
    // ReSharper disable InconsistentNaming
    internal class ContextFor_Should
    {
        [TestFixture]
        public class ContextFor_Cannot
        {
            [Test]
            public void Handle_Class_With_Nested_Constructor_Parameter()
            {
                // because NSubstitute does not support class with constructor parameter
                0.Invoking(x => Substitute.For<Class_With_One_Constructor_Parameter>()).Should().Throw<Exception>();

                // as long as they are public
                1.Invoking(x => Substitute.For<Class_With_One_Empty_Constructor>()).Should().Throw<Exception>();
            }
        }

        #region test classes

        // ReSharper disable UnusedMember.Local
        // ReSharper disable ClassNeverInstantiated.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable UnusedParameter.Local
        private class Class_Without_Constructor { }

        private class Class_With_One_Empty_Constructor
        {
            // ReSharper disable once EmptyConstructor
            public Class_With_One_Empty_Constructor() { }
        }

        private class Class_With_One_Constructor_Parameter
        {
            public IList<string> Param1 { get; }

            public Class_With_One_Constructor_Parameter(IList<string> param1)
            {
                Param1 = param1;
            }
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
            public ICloneable Cloneable { get; }

            public Class_With_Optional_Constructor_Parameter(IList<string> param1, ICloneable cloneable = null)
            {
                Cloneable = cloneable;
            }
        }

        private class Class_With_Two_Similar_Constructor_Parameter
        {
            public IList<string> Param1 { get; }
            public IList<string> Param2 { get; }

            public Class_With_Two_Similar_Constructor_Parameter(IList<string> param1, IList<string> param2)
            {
                Param1 = param1;
                Param2 = param2;
            }
        }

        private class Class_With_Two_Constructors
        {
            public IList<string> Param1 { get; }
            public IList<string> Param2 { get; }

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

        private class Class_With_Primitive_Constructor_Parameters
        {
            public string Name { get; }
            public int Age { get; }

            public Class_With_Primitive_Constructor_Parameters(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }
        // ReSharper restore once UnusedMember.Local
        // ReSharper restore ClassNeverInstantiated.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local
        // ReSharper restore UnusedParameter.Local

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
            ctx.Invoking(x => x.For<IFormattable>()).Should().Throw<ArgumentException>();

            // on named parameter
            ctx.Invoking(x => x.For<IList<string>>("this_does_not_exist")).Should().Throw<ArgumentException>();
        }

        [Test]
        public void Throw_Exception_On_Wrong_Expectation_On_Parameter_Type()
        {
            var ctx = new ContextFor<Class_With_One_Constructor_Parameter>();

            // wrong parameter type
            ctx.Invoking(x => x.For<IFormattable>("param1")).Should().Throw<InvalidCastException>();
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
            ctx.For<IList<string>>().IndexOf(Arg.Any<string>()).ReturnsForAnyArgs(4);
         
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();
            sut.Param1.IndexOf("foo").Should().Be(4);
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

            // lets see if the params got injected properly
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

            // lets see if the params got injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Param1.Should().Equal(param1a);
            sut.Param2.Should().Equal(param2a);
        }

        [Test]
        public void Use_Instances()
        {
            var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>();

            // now lets replace the clonable
            var cloneable = Substitute.For<ICloneable>();
            ctx.Use(cloneable);

            // lets see if the params got injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Cloneable.Should().Be(cloneable);

            // get a keynotfound exception if wrong parameter
            ctx.Invoking(c => c.Use("I am not a constructor parameter"))
                .Should().Throw<ArgumentException>();
        }

        [Test(Description = "test also primitives, not only substitutes")]
        public void Support_primitive_instances()
        {
            var ctx = new ContextFor<Class_With_Primitive_Constructor_Parameters>();
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Name.Should().Be(default(string));
            sut.Age.Should().Be(default(int));

            ctx.Use("Name");
            ctx.Use(42);

            sut = ctx.BuildSut();
            sut.Should().NotBeNull();
            sut.Name.Should().Be("Name");
            sut.Age.Should().Be(42);
        }

        [Test]
        public void Handle_Class_With_Two_Constructors_And_Use_Types()
        {
            var ctx = new ContextFor<Class_With_Two_Constructors>(typeof(IList<string>));

            // it always should return the first parameter
            var param1 = ctx.For<IList<string>>();
            param1.Should().NotBeNull();

            // lets see if the params got injected properly
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
            ctx.Invoking(c => c.For<IList<string>>("param2")).Should().Throw<ArgumentException>();
            param1a.Should().NotBeNull();

            // lets see if the params got injected properly
            var sut = ctx.BuildSut();
            sut.Should().NotBeNull();

            sut.Param1.Should().Equal(param1a);
            sut.Param2.Should().BeNull();
        }

        [Test,
         Issue("https://github.com/awesome-inc/NEdifis/issues/24",
             Title = "Add option `ctx.Use<T>`(string parameterName)` #24")]
        public void Support_using_multiple_instances_of_the_same_type()
        {
            var ctx = new ContextFor<Class_With_Two_Similar_Constructor_Parameter>();
            var list1 = new List<string>();
            var list2 = new List<string>();
            ctx.Use<IList<string>>(list2, nameof(Class_With_Two_Similar_Constructor_Parameter.Param2));
            ctx.Use<IList<string>>(list1, nameof(Class_With_Two_Similar_Constructor_Parameter.Param1));
        }

    }
}
