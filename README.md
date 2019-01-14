# NEdifis

[![Build status](https://ci.appveyor.com/api/projects/status/tghwql6ktsc9enqw?svg=true)](https://ci.appveyor.com/project/awesome-inc-build/nedifis)
[![NuGet](https://img.shields.io/nuget/v/NEdifis.svg?style=flat-square)](https://www.nuget.org/packages/NEdifis/)
[![NuGet](https://img.shields.io/nuget/dt/NEdifis.svg?style=flat-square)](https://www.nuget.org/packages/NEdifis/)
[![Coverage Status](https://coveralls.io/repos/github/awesome-inc/NEdifis/badge.svg?branch=develop)](https://coveralls.io/github/awesome-inc/NEdifis?branch=develop)

Framework for enhanced testing using NUnit and NSubstitute. This project contains classes and
attributes which can be used to simplify testing and enforcing conventions to your tests. It
helps to glue your implementation and tests together and reference each other.

## Who is Edifis?

Edifis is "the best architect in Alexandria", which according to Cleopatra is hardly saying much... And we have to
admit she's right when we look at the ramshackle structures built by Edifis and wonderfully illustrated
by Albert Uderzo...[more](https://www.asterix.com/en/portfolio/edifis/?portfolioCats=311 "Edifis")

## `ContextFor`

The `Context.For<>` is a builder for you sut. It uses NSubstitute to create a mock for all constructor parameter. The `Context.For<>` builds an empty sut with simple mocks for each constructor parameter.

```csharp
// arrange sut
var ctx = new ContextFor<Class_Without_Constructor>();
var sut = ctx.BuildSut();
```

In case you need to mock a method, the `For<>()` Method returns the mocked instantance for the constructor parameter with the given type.

```csharp
// arrange sut
var ctx = new ContextFor<Class_With_One_Constructor_Parameter>();
ctx.For<IList<string>>().IndexOf(Arg.Any<string>()).ReturnsForAnyArgs(4);
var sut = ctx.BuildSut();

sut.Param1.IndexOf("foo").Should().Be(4);
```

If there are two parameter with the same type, you can use the parameter name to get a specific constructor parameter mock.

```csharp
var ctx = new ContextFor<Class_With_Two_Constructors>();

// now we get it explicitly
var param1a = ctx.For<IList<string>>("param1");
var param2a = ctx.For<IList<string>>("param2");
```

Optional parameter are mocked by default. To avoid a mock for an optional parameter, a parameter can change the default behaviour.

```csharp
var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>(substituteOptionalParameter: false);
```

For replacing an entire instance instead of changing the mock behavior, the `Use<>()` Method can be used.

```csharp
```var ctx = new ContextFor<Class_With_Optional_Constructor_Parameter>();

// now lets replace the clonable
var cloneable = Substitute.For<ICloneable>();
ctx.Use(cloneable);

ctx.For<IClonable>().Should().Be(cloneable)
```

If you need to differentiate between parameters of the same type, add the parameter name. Note that using [nameof()](https://msdn.microsoft.com/en-us/library/dn986596.aspx) comes in handy here and makes the code more robust against name refactorings.

```csharp
var ctx = new ContextFor<Class_With_Two_Similar_Constructor_Parameter>();

var list2 = new List<string>();
ctx.Use<IList<string>>(list2, nameof(Class_With_Two_Similar_Constructor_Parameter.Param2));
```

## Attributes

The additional attributes helps your tests to tell more about themselves.

### `Because` Attribute

The `Because` attribute is used to give something a reason, because some attribute don't support
a reason by default like

```csharp
[ExcludeFromCodeCoverage]
[Because("I wrap system functionality which cannot be tested")]
public class HttpWrapper : IWrapHttp
{ }
```

The `Because` makes it easier to tell _why_ you have done something. Attribute are easier than
inspecting the file history and the attribute can be used for your convention tests (see below).

**Note:** Using the `Because` attribute on a production classes imposes a runtime dependency on `NEdifis` which you
might want to avoid (see [NEdifis/issues/19](https://github.com/awesome-inc/NEdifis/issues/19)).

### `Issue` Attribute

The issue attribute is used to add an issue and a description to a test (or fixture), especially
if it is a fix for a reported issue. An issue attribute can be used multiple times.

```csharp
[TestFixtureFor(typeof(TicketAttribute))]
[Issue("#4", Title = "Create an attribute to assign an issue id")]
[Issue("#13", Title = "a test should resolve or be related to multiple issues")]
// ReSharper disable once InconsistentNaming
internal class IssueAttribute_Should
{ }
```

### `TestFixtureFor` and `TestedBy`

The `TestFixtureFor` (which inherits from [TestFixture](https://github.com/nunit/docs/wiki/TestFixture-Attribute)) and `TestedBy` are pairing to make navigating between class and its fixture even easier.

```csharp
[TestedBy(typeof(IssueAttribute_Should))]
public class IssueAttribute : Attribute
{ }

[TestFixtureFor(typeof(IssueAttribute))]
// ReSharper disable once InconsistentNaming
internal class IssueAttribute_Should
{ }
```

There are convention tests which support a validation if each class has a `TestedBy` and a test class
has a `TestFixtureFor`.

**Note:** Since we advocate for [placing tests right beside production code](https://awesome-incremented.blogspot.de/2015/06/shipping-tests-along-with-production.html), we recommend using only `TestFixtureFor` to avoid a runtime dependency to NEdifis (see [NEdifis/issues/19](https://github.com/awesome-inc/NEdifis/issues/19)).

## Convention Tests

Convention tests are useful to make sure your classes and unit tests do not break your coding conventions. 
These conventions can be a simple naming convention or requiring to explain why you exclude some class from code coverage, i.e. if you use `ExcludeCodeFromCoverage` you must also provide a `Because`.

```csharp
internal class CheckConventions : ConventionBase
{
    public CheckConventions()
    {
        Conventions.AddRange(new IVerifyConvention[]
        {
            new ExcludeFromCodeCoverageClassHasBecauseAttribute(),
            new AllClassesNeedATest(),
            new ClassAndTestReferenceEachOther(),
            new TestClassesShouldEndWithShould(),
            new TestClassesShouldBePrivate()
        });
    }
}
```

You can create your own conventions implementing the `IVerifyConvention` Interface and adding the test to your `CheckConventions` Class.

```csharp
public interface IVerifyConvention
{
    Func<Type, bool> Filter { get; }
    void Verify(Type type);
}
```

## Trace and Debug "testing"

### `TestTraceListener`

The test trace listener can be used to verify if a method traced something. Sometimes there is a requirement like
*"in case an exception is caught, the exception should be logged to a file"*. When using [Trace](https://msdn.microsoft.com/en-us/library/system.diagnostics.trace(v=vs.110).aspx), the `TestTraceListener` is an easy way for testing these kinds of requirements. Here is an example:

```csharp
using (var ttl = new TestTraceListener())
{
    Trace.TraceError("here is a message");

    ttl.MessagesFor(TraceLevel.Error).Should().Contain("here is a message");
}
```

With the dispose pattern, the test trace listener is automatically removed from listeners during dispose. Therefore
the `using` statement is recommended. For debug redirect and testing you can also use the test trace listener because both classes [use the same listener collection]("https://msdn.microsoft.com/en-us/library/system.diagnostics.debug.listeners(v=vs.110).aspx").

```csharp
using (var ttl = new TestTraceListener())
{
    Debug.WriteLine("nice debug");

    ttl.MessagesFor(TraceLevel.Verbose).Should().Contain("nice debug");
}
```
