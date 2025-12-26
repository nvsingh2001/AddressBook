using AddressBook.UI;

namespace AddressBook.Tests;

[TestFixture]
public class InputValidatorTests
{
    private TextReader _originalIn = null!;
    private TextWriter _originalOut = null!;

    [SetUp]
    public void Setup()
    {
        _originalIn = Console.In;
        _originalOut = Console.Out;
    }

    [TearDown]
    public void Cleanup()
    {
        Console.SetIn(_originalIn);
        Console.SetOut(_originalOut);
    }

    [Test]
    public void GivenValidInput_WhenGetValidatedInput_ThenReturnsInput()
    {
        // Arrange
        const string simulatedInput = "ValidInput";
        
        Console.SetIn(new StringReader(simulatedInput));

        // Act
        var result = InputValidator.GetValidatedInput("Enter something:", input => true, "Error", true);

        // Assert
        Assert.That(result, Is.EqualTo("ValidInput"));
    }

    [Test]
    public void GivenInvalidThenValidInput_WhenGetValidatedInput_ThenRetriesAndReturnsValid()
    {
        // Arrange
        
        var simulatedInput = $"Bad{Environment.NewLine}Good";
        Console.SetIn(new StringReader(simulatedInput));

        Console.SetOut(new StringWriter());

        bool MyValidator(string input) => input != "Bad";

        // Act
        var result = InputValidator.GetValidatedInput("Enter something:", MyValidator, "Invalid input!", true);

        // Assert
        Assert.That(result, Is.EqualTo("Good"));
    }
}
