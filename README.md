[![Build status](https://ci.appveyor.com/api/projects/status/tghwql6ktsc9enqw?svg=true)](https://ci.appveyor.com/project/awesome-inc-build/nedifis) ![NuGet Version](https://img.shields.io/nuget/v/NEdifis.svg?style=flat-square) ![NuGet Version](https://img.shields.io/nuget/dt/NEdifis.svg?style=flat-square)

### Who is Edifis?

Edifis is "the best architect in Alexandria", which according to Cleopatra is hardly saying much... And we have to 
admit she's right when we look at the ramshackle structures built by Edifis and wonderfully illustrated 
by Albert Uderzo...[more](http://www.asterix.com/the-a-to-z-of-asterix/characters/edifis.html "Edifis")

# NEdifis

Framework for enhanced testing using NUnit and NSubstitute. This project contains classes and 
attributes which can be used to simplify testing and enforcing conventions to your tests. It 
helps to glue your implementation and tests together and reference each other.

## `ContextFor`
 
The `Context.For<>` is a builder for you sut. It uses NSubstitute to create a mock for all constructor parameter. The `Context.For<>` builds an empty sut with simple mocks for each constructor parameter.

    // arrange sut
	var ctx = new ContextFor<Class_Without_Constructor>();
	var sut = ctx.BuildSut();

In case you need to mock a method, the `For<>()` Method returns the mocked instantance for the constructor parameter with the given type.

    // arrange sut
    var ctx = new ContextFor<Class_With_One_Constructor_Parameter>();
    ctx.For<IList<string>>().IndexOf(Arg.Any<string>()).ReturnsForAnyArgs(4);
    var sut = ctx.BuildSut();

    sut.Param1.IndexOf("foo").Should().Be(4);

If there are two parameter with the same type, you can use the parameter name to get a specific constructor parameter mock.

	var ctx = new ContextFor<Class_With_Two_Constructors>();
	
	// now we get it explicitly
	var param1a = ctx.For<IList<string>>("param1");
	var param2a = ctx.For<IList<string>>("param2");
	
Optional parameter are mocked by default. To avoid a mock for an optional parameter, a parameter can change the default behaviour.

	var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>(substituteOptionalParameter: false);
	
For replacing an entire instance instead of changing the mock behaviour, the `Use<>()` Method can be used.

    var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>();

    // now lets replace the clonable
    var cloneable = Substitute.For<ICloneable>();
    ctx.Use(cloneable);

	ctx.For<IClonable>().Should().Be(cloneable)


## Attributes

The additional attributes helps your tests to tell more about themselfes.

### `Because` Attribute

The `Because` attribute is used to give something a reason, because some attribute don't support
a reason by default like

	[ExcludeFromCodeCoverage]
    [Because("I wrap system functionality which cannot be tested")]
	public class HttpWrapper : IWrapHttp
	{
	}

The `Because` makes it easier to tell _why_ you have done something. Attribute are easier than 
inspecting the file history and the attribute can be used for your convention tests (see below).


### `Ticket` Attribute

The ticket attribute is used to add the ticket and a description to a test, especially
if it is a fix for a reported issue.

    [TestFixtureFor(typeof(TicketAttribute))]
    [Ticket(4, Title = "Create an attribute to assign a ticket id")]
    // ReSharper disable once InconsistentNaming
    class TicketAttribute_Should
    {
    }

### `TestFixtureFor` and `TestedBy`

The TestFixtureFor (which inherits from TestFixture) and TestedBy are symetric to glue the test and 
the class.

    [TestedBy(typeof(TicketAttribute_Should))]
    public class TicketAttribute : Attribute
    {
	}

	[TestFixtureFor(typeof(TicketAttribute))]
    // ReSharper disable once InconsistentNaming
    public class TicketAttribute_Should
    {
	}

There are convention tests which support a validation if each class has a TestedBy and a test class
has a TestFixtureFor.

## Convention Tests

Convention tests are useful to make sure your classes and unit tests fulfils a coding convention. These conventions can be a simple naming convention or a requirement that if you use `ExcludeCodeFromCoverage` you must provide a `Because`.

    class CheckConventions : ConventionBase
    {
        protected override void Configure()
        {
            Conventions.Add(new ExcludeFromCodeCoverageClassHasBecauseAttribute());
            Conventions.Add(new AllClassesNeedATest());
            Conventions.Add(new ClassAndTestReferenceEachOther());
            Conventions.Add(new TestClassesShouldEndWithShould());
            Conventions.Add(new TestClassesShouldBePrivate());
        }
    }

You can create your own conventions implementing the `IVerifyConvention` Interface and adding the test to you `CheckConventions` Class.

    public interface IVerifyConvention
    {
        string HintOnFail { get; }
        bool FulfilsConvention(Type t);
    }


## Trace and Debug "testing"

### `TestTraceListener`

The test trace listener can be used to verify if a method traced something. Sometimes there is a requirement which 
says "in case an exception is caught, the exception should be logged to a file". The `TestTraceListener` can do this.

	using (var ttl = new TestTraceListener())
	{
		Trace.TraceError("here is a message");

		ttl.MessagesFor(TraceLevel.Error).Should().Contain("here is a message");
	}

With the dispose pattern, the test trace listener is automatically removed from listeners during dispose. Therefore
the `using`statement is recommended. For debug redirect and testing you can use the test trace listener because both
classes [use the same listener collection]("https://msdn.microsoft.com/en-us/library/system.diagnostics.debug.listeners(v=vs.110).aspx").

	using (var ttl = new TestTraceListener())
	{
		Debug.WriteLine("nice debug");

		ttl.MessagesFor(TraceLevel.Verbose).Should().Contain("nice debug");
	}




