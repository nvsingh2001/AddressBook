namespace AddressBook;

// Ability to create a Contacts in Address
// Book with first and last names, address,
// city, state, zip, phone number and
// email…

public class Contact(string firstName, string lastName,string phone,string email = "", string address = "",
    string city = "", string state = "", string zip = "")
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
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
}