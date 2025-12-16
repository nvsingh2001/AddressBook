using System.Collections;
using AddressBook.Models;
using AddressBook.Exceptions;

namespace AddressBook.Services;

public class ContactManager : IEnumerable<Contact>
{
    private readonly List<Contact> _contacts = new List<Contact>();

    public void AddContact(Contact contact)
    {
        if (ContainsContact(contact.FirstName + contact.LastName))
        {
            throw new DuplicateContactException(contact.FirstName + contact.LastName);
        }
        _contacts.Add(contact);
    }

    public bool TryGetContact(string name, out Contact contact)
    {
        foreach (var cont in _contacts)
        {
            if (string.Equals(cont.FirstName + cont.LastName, name, StringComparison.InvariantCultureIgnoreCase)) 
            {
                contact = cont;
                return true;
            }
        }
        contact = default!;
        return false;
    }

    public void DeleteContact(string name)
    {
        var contact = _contacts.FirstOrDefault(c => string.Equals(c.FirstName + c.LastName, name, StringComparison.InvariantCultureIgnoreCase));
        if (contact == null)
        {
            throw new ContactNotFoundException(name);
        }
        _contacts.Remove(contact);
    }

    public bool ContainsContact(string name)
    {
        return  _contacts.Any(cont => string.Equals(cont.FirstName + cont.LastName, name, StringComparison.InvariantCultureIgnoreCase));
    }
    
    public bool IsEmpty()
    {
        return _contacts.Count == 0;
    }

    public IEnumerator<Contact> GetEnumerator()
    {
        return _contacts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}