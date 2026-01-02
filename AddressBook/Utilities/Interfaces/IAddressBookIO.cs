namespace AddressBook.Services.Interfaces;
using AddressBook.Models;

public interface IAddressBookIo
{
    Dictionary<string, List<Contact>> ExtractData(AddressBookService addressBookService);
    Task WriteToTextFile(AddressBookService addressBookService, string path);
    Task ReadFromTextFile(AddressBookService addressBookService, string path);
}