namespace AddressBook.Services.Interfaces;
using AddressBook.Models;

public interface IAddressBookIo
{
    Task SaveDataAsync(AddressBookService addressBookService);
    Task LoadDataAsync(AddressBookService addressBookService);
}