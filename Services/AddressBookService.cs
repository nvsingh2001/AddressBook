using AddressBook.Exceptions;
using AddressBook.Models;

namespace AddressBook.Services;

public class AddressBookService
{
    private readonly Dictionary<string, ContactManager> _addressBooks = new Dictionary<string, ContactManager>();
    public  Dictionary<string, List<Contact>> CityDictionary { get; } = new Dictionary<string, List<Contact>>();
    public  Dictionary<string, List<Contact>> StateDictionary { get; } = new Dictionary<string, List<Contact>>();
    
    public void CreateAddressBook(string name)
    {
        if (_addressBooks.ContainsKey(name))
        {
            throw new DuplicateAddressBookException(name);
        }
        _addressBooks.Add(name, new ContactManager());
    }

    public void AddAddressBook(string name, ContactManager contactManager)
    {
        if (_addressBooks.ContainsKey(name))
        {
            throw new DuplicateAddressBookException(name);
        }
        _addressBooks.Add(name, contactManager);
    }

    public ContactManager GetAddressBook(string name)
    {
        if (!_addressBooks.ContainsKey(name))
        {
             throw new AddressBookNotFoundException(name);
        }
        return _addressBooks[name];
    }
    
    public bool ContainsAddressBook(string name)
    {
        return _addressBooks.ContainsKey(name);
    }
    
    public bool IsEmpty()
    {
        return _addressBooks.Count == 0;
    }

    public IEnumerable<ContactManager> GetAllAddressBooks()
    {
        return _addressBooks.Values;
    }
    
    public void AddContactByCityAndState(Contact contact){
        if (!CityDictionary.ContainsKey(contact.City.ToLower()))
        {
            CityDictionary[contact.City.ToLower()] = new List<Contact>();
        }

        if (!StateDictionary.ContainsKey(contact.State.ToLower()))
        {
            StateDictionary[contact.State.ToLower()] = new List<Contact>();
        }
        
        CityDictionary[contact.City.ToLower()].Add(contact);
        StateDictionary[contact.State.ToLower()].Add(contact);
    }

    public void RemoveContactByCityAndState(Contact contact)
    {
        CityDictionary[contact.City.ToLower()].Remove(contact);
        StateDictionary[contact.State.ToLower()].Remove(contact);
    }
}