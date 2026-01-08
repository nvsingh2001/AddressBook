using System.Text.Json;
using AddressBook.Services.Interfaces;
using AddressBook.Models;
using AddressBook.Services;

namespace AddressBook.Utilities.FileHandling.Json;

public class AddressBookJsonIo: IAddressBookIo
{
    private readonly string _filePath;

    public AddressBookJsonIo(string filePath)
    {
        _filePath = filePath;
    }

    private class ContactJsonModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
    }
    
    public async Task SaveDataAsync(AddressBookService addressBookService)
    {
        var data = addressBookService.AddressBooks
            .ToDictionary(
                book => book.Key,
                book => book.Value
                    .Select(contact => new ContactJsonModel
                    {
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        Address = contact.Address,
                        City = contact.City,
                        State = contact.State,
                        Zip = contact.Zip
                    })
                    .ToList()
            );

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var directory = Path.GetDirectoryName(_filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        await using var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fs, data, options);
    }
    
    
    public async Task LoadDataAsync(AddressBookService addressBookService)
    {
        if (!File.Exists(_filePath)) return;

        var json = File.OpenRead(_filePath);

        try
        {
            var data = await JsonSerializer.DeserializeAsync<
                Dictionary<string, List<ContactJsonModel>>
            >(json);

            if (data == null) return;

            foreach (var (bookName, contacts) in data)
            {
                ContactManager manager;

                if (!addressBookService.ContainsAddressBook(bookName))
                {
                    manager = new ContactManager();
                    addressBookService.AddAddressBook(bookName, manager);
                }
                else
                {
                    manager = addressBookService.GetAddressBook(bookName);
                }

                foreach (var record in contacts)
                {
                    var contact = new Contact(
                        record.FirstName,
                        record.LastName,
                        record.Phone,
                        record.Email,
                        record.Address,
                        record.City,
                        record.State,
                        record.Zip
                    );

                    if (!manager.ContainsContact(contact.FirstName + contact.LastName))
                    {
                        manager.AddContact(contact);
                        addressBookService.AddContactByCityAndState(contact);
                    }
                }
            }
        }
        catch (JsonException)
        {
            // Handle empty or invalid JSON
        }
    }
}