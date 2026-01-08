using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Services.Interfaces;
using CsvHelper;
using System.Globalization;

namespace AddressBook.Utilities.FileHandling.Csv;

public class AddressBookCsvIO : IAddressBookIo
{
    private readonly string _filePath;

    public AddressBookCsvIO(string filePath)
    {
        _filePath = filePath;
    }

    private class ContactCsvModel
    {
        public string AddressBookName { get; set; } = string.Empty;
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
        var data = addressBookService.AddressBooks.ToDictionary(book => book.Key, book => book.Value.ToList());
        
        // Flatten the data to include the AddressBook name
        var records = data.SelectMany(book => book.Value.Select(contact => new ContactCsvModel
        {
            AddressBookName = book.Key,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Phone = contact.Phone,
            Email = contact.Email,
            Address = contact.Address,
            City = contact.City,
            State = contact.State,
            Zip = contact.Zip
        })).ToList();

        var directory = Path.GetDirectoryName(_filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var writer = new StreamWriter(_filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(records);
    }

    public async Task LoadDataAsync(AddressBookService addressBookService)
    {
        if (!File.Exists(_filePath)) return;

        try 
        {
            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            var records = await csv.GetRecordsAsync<ContactCsvModel>().ToListAsync();
            
            foreach (var group in records.GroupBy(r => r.AddressBookName))
            {
                var bookName = group.Key;
                ContactManager currentManager;
                
                if (!addressBookService.ContainsAddressBook(bookName))
                {
                    currentManager = new ContactManager();
                    addressBookService.AddAddressBook(bookName, currentManager);
                }
                else
                {
                    currentManager = addressBookService.GetAddressBook(bookName);
                }
                
                foreach(var record in group)
                {
                    var contact = new Contact(record.FirstName, record.LastName, record.Phone, record.Email, record.Address, record.City, record.State, record.Zip);
                    if (!currentManager.ContainsContact(contact.FirstName + contact.LastName))
                    {
                        currentManager.AddContact(contact);
                        addressBookService.AddContactByCityAndState(contact);
                    }
                }
            }
        }
        catch (CsvHelperException)
        {
            // Handle CSV specific errors or empty files gracefully
        }
    }
}
