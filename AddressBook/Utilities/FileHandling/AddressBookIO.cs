using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Services.Interfaces;

namespace AddressBook.Utilities.FileHandling;

public class AddressBookIO : IAddressBookIo
{
    private const string AddressBookDelimiter = "###ADDRESSBOOK:";
    private const string ContactFieldDelimiter = ",";

    public Dictionary<string, List<Contact>> ExtractData(AddressBookService addressBookService)
    {
       return addressBookService.AddressBooks.ToDictionary(book => book.Key,
            book => book.Value.ToList());
    }

    public async Task WriteToTextFile(AddressBookService addressBookService, string path)
    {
        using var writer = new StreamWriter(path);
        var data = ExtractData(addressBookService);
        foreach (var book in data)
        {
            writer.WriteLine($"{AddressBookDelimiter}{book.Key}");
            foreach (var contact in book.Value)
            {
                var line = string.Join(ContactFieldDelimiter,
                    contact.FirstName, contact.LastName, contact.Phone, contact.Email,
                    contact.Address, contact.City, contact.State, contact.Zip);
               await writer.WriteLineAsync(line);
            }
        }
    }

    public async Task ReadFromTextFile(AddressBookService addressBookService, string path)
    {
        if (!File.Exists(path)) return;

        using var reader = new StreamReader(path);
        string? line;
        ContactManager? currentManager = null;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line.StartsWith(AddressBookDelimiter))
            {
                var bookName = line.Substring(AddressBookDelimiter.Length);
                if (!addressBookService.ContainsAddressBook(bookName))
                {
                    currentManager = new ContactManager();
                    addressBookService.AddAddressBook(bookName, currentManager);
                }
                else
                {
                    currentManager = addressBookService.GetAddressBook(bookName);
                }

                continue;
            }

            if (currentManager != null)
            {
                var parts = line.Split(ContactFieldDelimiter);
                if (parts.Length >= 8)
                {
                    var contact = new Contact(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6],
                        parts[7]);
                    if (!currentManager.ContainsContact(contact.FirstName + contact.LastName))
                    {
                        currentManager.AddContact(contact);
                        addressBookService.AddContactByCityAndState(contact);
                    }
                }
            }
        }
    }
}