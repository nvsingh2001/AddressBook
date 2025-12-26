using System.Collections;
using AddressBook.Exceptions;
using AddressBook.Models;
using AddressBook.Services.Interfaces;

namespace AddressBook.Services;

public class ContactManager : IContactManager
{
    public List<Contact> Contacts { get; } = new();

    public void AddContact(Contact contact)
    {
        if (ContainsContact(contact.FirstName + contact.LastName))
            throw new DuplicateContactException(contact.FirstName + contact.LastName);
        Contacts.Add(contact);
    }

    public bool TryGetContact(string name, out Contact contact)
    {
        foreach (var cont in Contacts)
            if (string.Equals(cont.FirstName + cont.LastName, name, StringComparison.InvariantCultureIgnoreCase))
            {
                contact = cont;
                return true;
            }

        contact = default!;
        return false;
    }

    public Contact DeleteContact(string name)
    {
        var contact = Contacts.FirstOrDefault(c =>
            string.Equals(c.FirstName + c.LastName, name, StringComparison.InvariantCultureIgnoreCase));
        if (contact == null) throw new ContactNotFoundException(name);
        Contacts.Remove(contact);

        return contact;
    }

    public bool ContainsContact(string name)
    {
        return Contacts.Any(cont =>
            string.Equals(cont.FirstName + cont.LastName, name, StringComparison.InvariantCultureIgnoreCase));
    }

    public bool IsEmpty()
    {
        return Contacts.Count == 0;
    }

    public IEnumerator<Contact> GetEnumerator()
    {
        return Contacts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Sort()
    {
        Contacts.Sort();
    }

    public void Sort(IComparer<Contact> comparer)
    {
        Contacts.Sort(comparer);
    }
}