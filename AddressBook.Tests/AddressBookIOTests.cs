using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Utilities.FileHandling;

namespace AddressBook.Tests;

[TestFixture]
public class AddressBookIOTests
{
    private string _tempFilePath;
    private AddressBookService _service;
    private AddressBookIO _addressBookIO;

    [SetUp]
    public void Setup()
    {
        _tempFilePath = Path.GetTempFileName();
        _service = new AddressBookService();
        _addressBookIO = new AddressBookIO();
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }

    [Test]
    public void WriteAndReadTextFile_ShouldPersistDataCorrectly()
    {
        // Arrange
        string bookName = "TestBook";
        var contact = new Contact(
            "John", "Doe", "1234567890", "john@example.com", 
            "123 Main St", "New York", "NY", "10001"
        );
        
        var manager = new ContactManager();
        manager.AddContact(contact);
        _service.AddAddressBook(bookName, manager);
        _service.AddContactByCityAndState(contact);

        // Act - Write
        _addressBookIO.WriteToTextFile(_service, _tempFilePath);

        // Assert - File Exists
        Assert.That(File.Exists(_tempFilePath), Is.True);

        // Act - Read into new service
        var newService = new AddressBookService();
        _addressBookIO.ReadFromTextFile(newService, _tempFilePath);

        // Assert - Verify Data
        Assert.That(newService.ContainsAddressBook(bookName), Is.True);
        var loadedManager = newService.GetAddressBook(bookName);
        Assert.That(loadedManager.ContainsContact("JohnDoe"), Is.True);
        
        loadedManager.TryGetContact("JohnDoe", out Contact loadedContact);
        Assert.That(loadedContact.Phone, Is.EqualTo("1234567890"));
        Assert.That(loadedContact.City, Is.EqualTo("New York"));

        // Verify City/State dictionaries populated (if logic added to ReadFromTextFile)
        Assert.That(newService.GetCountOfContactByCityAndState("New York", null), Is.EqualTo(1));
    }
}