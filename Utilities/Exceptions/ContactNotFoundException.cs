namespace AddressBook.Exceptions;

public class ContactNotFoundException : AddressBookException
{
    public ContactNotFoundException(string name) 
        : base($"Contact with name '{name}' was not found.")
    {
    }
}
