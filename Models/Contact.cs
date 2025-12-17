namespace AddressBook.Models;

// Ability to create a Contacts in Address
// Book with first and last names, address,
// city, state, zip, phone number and
// email…

public class Contact(string firstName, string lastName,string phone,string email = "", string address = "",
    string city = "", string state = "", string zip = "") : IComparable<Contact>
{
    public string FirstName { get;} = firstName;
    public string LastName { get;} = lastName;
    public string Address { get; set; } = address;
    public string City { get; set; } = city;
    public string State { get; set; } = state;
    public string Zip { get; set; } = zip;
    public string Phone { get; set; } = phone;
    public string Email { get; set; } = email;

    public override string ToString()
    {
        return $"{FirstName} {LastName} {Phone} {Email} {Address} {City} {State} {Zip}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Contact other &&
               FirstName.Equals(other.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
               LastName.Equals(other.LastName, StringComparison.InvariantCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }

    public int CompareTo(Contact? other)
    {
        if (other is null) return 1;

        int firstNameCompare = string.Compare(
            FirstName,
            other.FirstName,
            StringComparison.InvariantCultureIgnoreCase
        );

        if (firstNameCompare != 0)
            return firstNameCompare;

        return string.Compare(
            LastName,
            other.LastName,
            StringComparison.InvariantCultureIgnoreCase
        );
    }
}