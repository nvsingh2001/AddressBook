using AddressBook.Exceptions;
using AddressBook.Services;

namespace AddressBook.Tests;

[TestFixture]
public class Tests
{
    [OneTimeSetUp]
    public void Setup()
    {
        _addressBookService = new AddressBookService();

        var addressBookName = "AddressBook1";

        _addressBookService.AddAddressBook(addressBookName, new ContactManager());
    }

    private AddressBookService _addressBookService;

    [Test]
    [TestCase("AddressBook2")]
    public void WhenCallingContainsAddressBook_ThenReturnsFalse(string name)
    {
        // Arrange
        const bool expected = false;

        // Act
        var actual = _addressBookService.ContainsAddressBook(name);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("AddressBook1")]
    public void WhenCallingContainsAddressBook_ThenReturnsTrue(string name)
    {
        // Arrange
        const bool expected = true;

        // Act
        var actual = _addressBookService.ContainsAddressBook(name);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void GivenDuplicateAddressBook_WhenCreateAddressBook_ThenThrowsDuplicateAddressBookException()
    {
        // Arrange
        const string existingName = "AddressBook1";

        var expectedException = new DuplicateAddressBookException(existingName);

        // Act
        var ex = Assert.Throws<DuplicateAddressBookException>(() =>
            _addressBookService.CreateAddressBook(existingName));

        // Assert
        Assert.That(ex.Message, Is.EqualTo(expectedException.Message));
    }
}