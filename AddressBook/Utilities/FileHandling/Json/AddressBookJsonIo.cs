using System.Text.Json;
using AddressBook.Services.Interfaces;
using AddressBook.Models;
using AddressBook.Services;

namespace AddressBook.Utilities.FileHandling.Json;

public class AddressBookJsonIo: IAddressBookIo
{
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
    
    public Dictionary<string, List<Contact>> ExtractData(AddressBookService addressBookService)
    {
        return addressBookService.AddressBooks.ToDictionary(book => book.Key,
            book => book.Value.ToList());
    }
    
    
    public void WriteToTextFile(AddressBookService addressBookService, string path)
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

        File.WriteAllText(path, JsonSerializer.Serialize(data, options));
    }
    
    
    public void ReadFromTextFile(AddressBookService addressBookService, string path)
    {
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);

        var data = JsonSerializer.Deserialize<
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
}