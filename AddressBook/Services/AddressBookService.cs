using AddressBook.Exceptions;
using AddressBook.Models;
using AddressBook.Services.Interfaces;
using AddressBook.Utilities.FileHandling;

namespace AddressBook.Services;

public class AddressBookService : IAddressBookService
{
    private readonly AddressBookIO _addressBookIo = new();
    private const string DataFilePath = "Data/addressbook.txt";

    public Dictionary<string, ContactManager> AddressBooks { get; } = new();
    public Dictionary<string, List<Contact>> CityDictionary { get; } = new();
    public Dictionary<string, List<Contact>> StateDictionary { get; } = new();

    public void CreateAddressBook(string name)
    {
        if (AddressBooks.ContainsKey(name)) throw new DuplicateAddressBookException(name);
        AddressBooks.Add(name, new ContactManager());
    }

    public void AddAddressBook(string name, ContactManager contactManager)
    {
        if (AddressBooks.ContainsKey(name)) throw new DuplicateAddressBookException(name);
        AddressBooks.Add(name, contactManager);
    }

    public ContactManager GetAddressBook(string name)
    {
        if (!AddressBooks.ContainsKey(name)) throw new AddressBookNotFoundException(name);
        return AddressBooks[name];
    }

    public bool ContainsAddressBook(string name)
    {
        return AddressBooks.ContainsKey(name);
    }

    public bool IsEmpty()
    {
        return AddressBooks.Count == 0;
    }

    public IEnumerable<ContactManager> GetAllAddressBooks()
    {
        return AddressBooks.Values;
    }

    public IEnumerable<string> GetAddressBookNames()
    {
        return AddressBooks.Keys;
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

    public void SaveData()
    {
        var directory = Path.GetDirectoryName(DataFilePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        _addressBookIo.WriteToTextFile(this, DataFilePath);
    }

    public void LoadData()
    {
        if (File.Exists(DataFilePath))
        {
            _addressBookIo.ReadFromTextFile(this, DataFilePath);
        }
    }
}