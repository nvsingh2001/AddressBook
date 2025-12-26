using AddressBook.Exceptions;
using AddressBook.Models;
using AddressBook.Services;

namespace AddressBook.Tests;

[TestFixture]
public class ContactManagerTests
{
    private ContactManager _contactManager;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _contactManager = new ContactManager();
        
        var contact = new Contact("Naman", "Singh", "+917908254373", "nvsingh2001@hotmail.com");
        _contactManager.AddContact(contact);
    }
    
    [Test]
    public void GivenDuplicateContact_WhenAddingContactToContactManager_ThenThrowsDuplicateContactException()
    {
        // Arrange
        var exsistingContact = new Contact("Naman", "Singh", "+917908254373", "nvsingh2001@hotmail.com");
        
        var expectedException = new DuplicateContactException(exsistingContact.FirstName + exsistingContact.LastName);
        
        // Act
        var ex = Assert.Throws<DuplicateContactException>(() => _contactManager.AddContact(exsistingContact));
        
        //Assert
        Assert.That(ex.Message, Is.EqualTo(expectedException.Message));
    }

    [Test]
    public void GivenNonExistingContactName_WhenCallingTryGetContact_ThenReturnsFalse()
    {
        // Arrange
        const string nonExistingContactName = "AnkitKumar";

        // Act
        var actual = _contactManager.TryGetContact(nonExistingContactName, out var contact);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.False);
            Assert.That(contact, Is.Null, $"Contact should be null for name: {nonExistingContactName}");
        });
    }
    
    [Test]
    public void GivenExistingContactName_WhenCallingTryGetContact_ThenReturnsTrue()
    {
        // Arrange
        const string existingContactName = "NamanSingh";
        
        // Act
        var actual = _contactManager.TryGetContact(existingContactName, out var contact);
        
        // Assert
        Assert.That(actual, Is.True);
    }

    [Test]
    public void GivenMultipleNonExistingContactsName_WhenCallingTryGetContact_ThenReturnsFalse()
    {
        // Arrange
        var names = new string[] { "Naman", "AnkitKumar", "GouravKarmarkar" };

        // Act + Assert
        Assert.Multiple(() =>
        {
            foreach (var name in names)
            {
                var actual = _contactManager.TryGetContact(name, out var contact);

                Assert.That(actual, Is.False, $"Expected false for name: {name}");
                Assert.That(contact, Is.Null, $"Contact should be null for name: {name}");
            }
        });
    }
}