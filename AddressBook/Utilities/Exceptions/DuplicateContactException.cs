namespace AddressBook.Exceptions;

public class DuplicateContactException : AddressBookException
{
    public DuplicateContactException(string name)
        : base($"Contact with name '{name}' already exists.")
    {
    }
}