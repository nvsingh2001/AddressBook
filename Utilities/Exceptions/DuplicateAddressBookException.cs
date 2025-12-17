namespace AddressBook.Exceptions;

public class DuplicateAddressBookException : AddressBookException
{
    public DuplicateAddressBookException(string name) 
        : base($"Address Book with name '{name}' already exists.")
    {
    }
}
