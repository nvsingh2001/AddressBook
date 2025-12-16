using System.Collections;

namespace AddressBook;

internal class Contacts(int capacity)
{
    private readonly List<Contact> _contacts = new List<Contact>();

    public void AddContact(Contact contact)
    {
        _contacts.Add(contact);
    }

    public bool TryGetContact(string name, out Contact contact)
    {
        foreach (var cont in _contacts)
        {
            if (cont.FirstName + cont.LastName == name) 
            {
                contact = cont;
                return true;
            }
        }
        contact = default!;
        return false;
    }

    public void PrintAllContacts()
    {
        foreach (var contact in _contacts)
        {
            Console.WriteLine(contact.ToString());
        }
    }

    public bool DeleteContact(string name)
    {
        foreach(var contact in  _contacts)
        {
            if (contact.FirstName + contact.LastName == name)
            {
                _contacts.Remove(contact);
                return true;
            }
        }
        return false;
    }

    public bool ContainsContact(string name)
    {
        return  _contacts.Any(cont => cont.FirstName + cont.LastName == name);
    }
    
    public bool IsEmpty()
    {
        return _contacts.Count == 0;
    }
}