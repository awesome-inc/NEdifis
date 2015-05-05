# NEdifis

[![Build status](https://ci.appveyor.com/api/projects/status/tghwql6ktsc9enqw?svg=true)](https://ci.appveyor.com/project/awesome-inc-build/nedifis) ![NuGet Version](https://img.shields.io/nuget/v/NEdifis.svg?style=flat-square) ![NuGet Version](https://img.shields.io/nuget/dt/NEdifis.svg?style=flat-square)

Framework for enhanced testing using NUnit and NSubstitute. This project contains classes and 
attributes which can be used to simplify testing and enforcing conventions to your tests. It 
helps to glue your implementation and tests together and reference each other.

## `ContextFor`

**TODO**: Documentation of this awesome class implementing a builder pattern 


## Attributes

The additional attributes helps your tests to tell more about themselfes.

### `Because` Attribute

The `Because` attribute is used to give something a reason, because some attribute don't support
a reason by default like

	[ExcludeFromCodeCoverage]
    [Because("I wrap a system functionality which cannot be tested")]
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

**Todo**: The prototypes for convention tests need to be redesigned and are planned for version 0.3