namespace AddressBook.Exceptions;

public class AddressBookNotFoundException : AddressBookException
{
    public AddressBookNotFoundException(string name)
        : base($"Address Book with name '{name}' was not found.")
    {
    }
}