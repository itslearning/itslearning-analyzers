# Rules reference

The following table contains rules used for code analysis. All are described in detail further on in this document.

| Id     | Severity   | Title |
|--------|------------|-------|
|ITSA1000| :no_entry: | Test file name should have a standard ending.
|ITSA1001| :no_entry: | Test class name should have a standard ending.
|ITSA1002| :no_entry: | Test method name should have a standard format.
|ITSA1003| :no_entry: | Test project name should have a standard ending.
|ITSA1004| :no_entry: | Test method should not contain conditional statements like if, switch etc.
|ITSA1005| :warning:  | Test method should not contain any other comment than `// arrange`, `// act`, `// assert`.

## ITSA1000 Test file name should have a standard ending. 

#### Severity:

:no_entry: Error

#### Category:

Naming

#### Description:

Test file names should have a standardized format to distinguish them from test utility files. The allowed format is '*Tests.cs' for every file that contains one or more test fixture classes.

#### Example of correct use:

```
CalculatorTests.cs
CalculatorUnitTests.cs
```

#### Example of violation:

```
CalculatorTest.cs
CalculatorTesting.cs
CalculatorFixture.cs
```

## ITSA1001 Test class name should have a standard ending.

#### Severity:

:no_entry: Error

#### Category:

Naming

#### Description:

Test class names should have a standardized format to distinguish them from test utility classes. The allowed format is '*Tests' for every class that is marked a test fixture.

#### Example of correct use:

```
CalculatorTests
CalculatorUnitTests
```

#### Example of violation:

```
CalculatorTest
CalculatorTesting
CalculatorFixture
```

## ITSA1002 Test method name should have a standard format.

#### Severity:

:no_entry: Error

#### Category:

Naming

#### Description:

Test method names should have a standardized format to distinguish them from test utility classes. The allowed format is '{PRECONDITIONS}_Should{EFFECT}' for every method marked as a test method.

#### Example of correct use:

```csharp
[Test]
public void TestMethod_ShouldReturnCorrectResult() {}

[Test]
public void TestMethod_ShouldBeCorrect() {}

[TestCase]
public void TestMethod_WhenSomething_ShouldBeCorrect() {}

[TestCase]
public void TestMethod_WhenSomething_Should_BeCorrect() {}
```

#### Example of violation:

```csharp
[Test]
public void TestMethod() {}

[Test]
public void TestMethodShouldBeCorrect() {}

[TestCase]
public void TestMethod_IsCorrect() {}
```

## ITSA1003 Test project name should have a standard ending.

#### Severity:

:no_entry: Error

#### Category:

Naming

#### Description:

Test project names should have a standardized format to distinguish them from test utility files. The allowed format is: '*.(Unit|Integration)Tests' for every project that contains test fixtures.

#### Example of correct use:

```
Calculator.UnitTests
Calculator.IntegrationTests
Itslearning.Calculator.UnitTests
Itslearning.Calculator.IntegrationTests
```

#### Example of violation:

```
Calculator.Test
CalculatorTest
CalculatorTests
Calculator.Tests
Calculator.Tests.Fixture1
Tests.Calculator
```

## ITSA1004 Test method should not contain conditional statements like if, switch etc.

#### Severity:

:no_entry: Error

#### Category:

Design

#### Description:

Test methods should not contain complex logic that is the subject of testing itself. Try splitting particular cases into dedicated test methods.

#### Example of correct use:

```
[Test]
public void TestMethod() 
{
}

[Test]
public void TestMethodThatCallsOtherMethod() 
{
    ConditionalsInCalledMethodsAreCurrentlyNotDetected();
}

private void ConditionalsInCalledMethodsAreCurrentlyNotDetected()
{
    if (true) return;
}
```

#### Example of violation:

```
[Test]
public void TestMethod() 
{
    if (true) {}

    switch (1) { case 1: break; }

    var x = true ? 1 : 0; 
    System.Console.WriteLine(x);
}
```

## ITSA1005 Test method should not contain any other comment than `// arrange`, `// act`, `// assert`.

#### Severity:

:no_entry: Warning

#### Category:

Maintainability

#### Description:

Commenting test steps indicates substantial test method complexity. Unit tests should be simple. Consider extracting arrangement steps into some test utility.

#### Example of correct use:

```csharp
[Test]
public void TestMethod()
{
    // arrange
 
    // Act

    //  assert
}
```

#### Example of violation:

```csharp
[Test]
public void TestMethod()
{
    //
 
    /* */

    /**/

    // do some setup arrangement

    /* do some setup arrangement */

    /* 
        do some setup arrangement
        to prepare for tests 
    */

    // arrangements

    // arrange that

    // Act on system under test

    // assertions
}
```