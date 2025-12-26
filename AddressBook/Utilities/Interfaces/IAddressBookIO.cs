namespace AddressBook.Services.Interfaces;
using AddressBook.Models;

public interface IAddressBookIo
{
    Dictionary<string, List<Contact>> ExtractData(AddressBookService addressBookService);
    void WriteToTextFile(AddressBookService addressBookService, string path);
    void ReadFromTextFile(AddressBookService addressBookService, string path);
}