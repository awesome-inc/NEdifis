# NEdifis

Framework for enhanced testing using NUnit and NSubstitute. This project contains classes and 
attributes which can be used to simplify testing and enforcing conventions to your tests. It 
helps to glue your implementation and tests together and reference each other.

## Attributes

The additional attributes helps your tests to tell more about themselfes.

### `Because` Attribute

The because attribute is used to give something a reason, because some attribute don't support
a reason by default.

	[ExcludeFromCodeCoverage]
    [Because("I wrap a system functionality which cannot be tested")]
	public class HttpWrapper : IWrapHttp
	{
	}

The `Because` makes it easier to tell _why_ you have done something. Attribute are easier than 
inspecting the file history and the attribute can be used for your convention tests (see below).

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