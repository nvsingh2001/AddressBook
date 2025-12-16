namespace AddressBook;

internal class Contacts(int capacity)
{
    private Contact[] _contacts  = new Contact[capacity];
    private int _capacity = capacity;
    public int NumOfContacts { get; private set; } = 0;

    public void AddContact(Contact contact)
    {
        if (NumOfContacts == _capacity)
        {
            Contact[] newContacts = new Contact[_capacity * 2];
            Array.Copy(_contacts, 0, newContacts, 0, NumOfContacts);
            _contacts = newContacts;
            _capacity *= 2;
        }
        
        _contacts[NumOfContacts++] = contact;
    }

    public bool TryGetContact(string name, out Contact contact)
    {
        foreach (var cont in _contacts)
        {
            if (cont != null)
            {
                if (cont.FirstName + cont.LastName == name) 
                {
                    contact = cont;
                    return true;
                }
            }
        }
        contact = default!;
        return false;
    }

    public void PrintAllContacts()
    {
        foreach (Contact contact in _contacts)
        {
            if (contact != null!)
            {
                Console.WriteLine(contact.ToString());
            }
        }
    }

    public bool DeleteContact(string name)
    {
        for(int i = 0; i < _capacity; i++)
        {
            if (_contacts[i] == null)
            {
                continue;
            }
            
            if (_contacts[i].FirstName + _contacts[i].LastName == name)
            {
                _contacts[i] = null;
                NumOfContacts--;
                return true;
            }
        }
        return false;
    }
    
    public bool IsEmpty()
    {
        return NumOfContacts <= 0;
    }
}