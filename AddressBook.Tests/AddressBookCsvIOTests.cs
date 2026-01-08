using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Utilities.FileHandling.Csv;

namespace AddressBook.Tests;

[TestFixture]
public class AddressBookCsvIOTests
{
    private string _tempFilePath;
    private AddressBookService _service;
    private AddressBookCsvIO _addressBookCsvIO;

    [SetUp]
    public void Setup()
    {
        _tempFilePath = Path.GetTempFileName();
        _service = new AddressBookService();
        _addressBookCsvIO = new AddressBookCsvIO(_tempFilePath);
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
    public async Task WriteAndReadTextFile_ShouldPersistDataCorrectly()
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
        await _addressBookCsvIO.SaveDataAsync(_service);

        // Assert - File Exists
        Assert.That(File.Exists(_tempFilePath), Is.True);
        
        // Assert - File Content looks like CSV
        string fileContent = File.ReadAllText(_tempFilePath);
        Assert.That(fileContent, Does.Contain("AddressBookName,FirstName,LastName,Phone,Email,Address,City,State,Zip"));
        Assert.That(fileContent, Does.Contain("TestBook,John,Doe,1234567890,john@example.com,123 Main St,New York,NY,10001"));

        // Act - Read into new service
        var newService = new AddressBookService();
        await _addressBookCsvIO.LoadDataAsync(newService);

        // Assert - Verify Data
        Assert.That(newService.ContainsAddressBook(bookName), Is.True);
        var loadedManager = newService.GetAddressBook(bookName);
        Assert.That(loadedManager.ContainsContact("JohnDoe"), Is.True);
        
        loadedManager.TryGetContact("JohnDoe", out Contact loadedContact);
        Assert.That(loadedContact.Phone, Is.EqualTo("1234567890"));
        Assert.That(loadedContact.City, Is.EqualTo("New York"));

        // Verify City/State dictionaries populated
        Assert.That(newService.GetCountOfContactByCityAndState("New York", null), Is.EqualTo(1));
    }
}
