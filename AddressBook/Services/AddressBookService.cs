using AddressBook.Exceptions;
using AddressBook.Models;
using AddressBook.Services.Interfaces;

namespace AddressBook.Services;

public class AddressBookService : IAddressBookService
{
    private readonly Dictionary<string, ContactManager> _addressBooks = new();
    public Dictionary<string, List<Contact>> CityDictionary { get; } = new();
    public Dictionary<string, List<Contact>> StateDictionary { get; } = new();

    public void CreateAddressBook(string name)
    {
        if (_addressBooks.ContainsKey(name)) throw new DuplicateAddressBookException(name);
        _addressBooks.Add(name, new ContactManager());
    }

    public void AddAddressBook(string name, ContactManager contactManager)
    {
        if (_addressBooks.ContainsKey(name)) throw new DuplicateAddressBookException(name);
        _addressBooks.Add(name, contactManager);
    }

    public ContactManager GetAddressBook(string name)
    {
        if (!_addressBooks.ContainsKey(name)) throw new AddressBookNotFoundException(name);
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

    public IEnumerable<string> GetAddressBookNames()
    {
        return _addressBooks.Keys;
    }

    public void AddContactByCityAndState(Contact contact)
    {
        if (!CityDictionary.ContainsKey(contact.City.ToLower()))
            CityDictionary[contact.City.ToLower()] = new List<Contact>();

        if (!StateDictionary.ContainsKey(contact.State.ToLower()))
            StateDictionary[contact.State.ToLower()] = new List<Contact>();

        CityDictionary[contact.City.ToLower()].Add(contact);
        StateDictionary[contact.State.ToLower()].Add(contact);
    }

    public void RemoveContactByCityAndState(Contact contact)
    {
        if (CityDictionary.ContainsKey(contact.City.ToLower())) CityDictionary[contact.City.ToLower()].Remove(contact);

        if (StateDictionary.ContainsKey(contact.State.ToLower()))
            StateDictionary[contact.State.ToLower()].Remove(contact);
    }

    public int GetCountOfContactByCityAndState(string? city, string? state)
    {
        var count = 0;
        if (city != null && state != null)
        {
            var results = new List<Contact>();
            StateDictionary.TryGetValue(state.ToLower(), out results);
            if (results != null && results.Count > 0)
                foreach (var result in results)
                    if (result.City.Equals(city, StringComparison.InvariantCultureIgnoreCase))
                        count++;
        }
        else
        {
            if (city != null)
                if (CityDictionary.ContainsKey(city.ToLower()))
                    count += CityDictionary[city.ToLower()].Count;

            if (state != null)
                if (StateDictionary.ContainsKey(state.ToLower()))
                    count += StateDictionary[state.ToLower()].Count;
        }

        return count;
    }
}