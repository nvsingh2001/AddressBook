using AddressBook.Models;

namespace AddressBook.Services.Interfaces;

public interface IAddressBookService
{
    // Properties for accessing dictionaries if needed by UI (though preferably encapsulation should hide these)
    Dictionary<string, List<Contact>> CityDictionary { get; }
    Dictionary<string, List<Contact>> StateDictionary { get; }
    void CreateAddressBook(string name);
    void AddAddressBook(string name, ContactManager contactManager);
    ContactManager GetAddressBook(string name);
    IEnumerable<ContactManager> GetAllAddressBooks();
    IEnumerable<string> GetAddressBookNames();
    bool ContainsAddressBook(string name);
    bool IsEmpty();

    void AddContactByCityAndState(Contact contact);
    void RemoveContactByCityAndState(Contact contact);
    int GetCountOfContactByCityAndState(string? city, string? state);
}