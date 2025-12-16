using AddressBook.Exceptions;

namespace AddressBook.Services;

public class AddressBookService
{
    private readonly Dictionary<string, ContactManager> _addressBooks = new Dictionary<string, ContactManager>();

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
}