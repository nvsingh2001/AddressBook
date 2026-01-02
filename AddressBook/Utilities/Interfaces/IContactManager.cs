using AddressBook.Models;

namespace AddressBook.Services.Interfaces;

public interface IContactManager : IEnumerable<Contact>
{
    void AddContact(Contact contact);
    bool TryGetContact(string name, out Contact contact);
    Contact DeleteContact(string name);
    bool ContainsContact(string name);
    bool IsEmpty();
    void Sort();
    void Sort(IComparer<Contact> comparer);
}