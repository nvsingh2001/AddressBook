using AddressBook.Models;

namespace AddressBook.UI;

public static class TablePrinter
{
    public static void PrintContacts(IEnumerable<Contact> contacts)
    {
        var contactList = contacts.ToList();
        if (contactList.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        const int firstNameWidth = 12;
        const int lastNameWidth = 12;
        const int phoneWidth = 16;
        const int emailWidth = 26;
        const int addressWidth = 20;
        const int cityWidth = 14;
        const int stateWidth = 20;
        const int zipWidth = 8;

        int totalWidth = firstNameWidth + lastNameWidth + phoneWidth + emailWidth + 
                         addressWidth + cityWidth + stateWidth + zipWidth + 17;

        Console.Clear();
        MenuManager.PrintWelcomeScreen();
        Console.WriteLine("╔" + new string('═', totalWidth) + "╗");

        Console.WriteLine("║ " +
            PadRight("First Name", firstNameWidth) + "│ " +
            PadRight("Last Name", lastNameWidth) + "│ " +
            PadRight("Phone", phoneWidth) + "│ " +
            PadRight("Email", emailWidth) + "│ " +
            PadRight("Address", addressWidth) + "│ " +
            PadRight("City", cityWidth) + "│ " +
            PadRight("State", stateWidth) + "│ " +
            PadRight("Zip", zipWidth) + "  ║");

        Console.WriteLine("╠" + new string('═', totalWidth) + "╣");

        foreach (var contact in contactList)
        {
            Console.WriteLine("║ " +
                PadRight(Truncate(contact.FirstName, firstNameWidth), firstNameWidth) + "│ " +
                PadRight(Truncate(contact.LastName, lastNameWidth), lastNameWidth) + "│ " +
                PadRight(Truncate(contact.Phone, phoneWidth), phoneWidth) + "│ " +
                PadRight(Truncate(contact.Email, emailWidth), emailWidth) + "│ " +
                PadRight(Truncate(contact.Address, addressWidth), addressWidth) + "│ " +
                PadRight(Truncate(contact.City, cityWidth), cityWidth) + "│ " +
                PadRight(Truncate(contact.State, stateWidth), stateWidth) + "│ " +
                PadRight(Truncate(contact.Zip, zipWidth), zipWidth) + "  ║");
        }

        Console.WriteLine("╚" + new string('═', totalWidth) + "╝");
        Console.WriteLine($"\nTotal contacts: {contactList.Count}");
    }

    public static void PrintAddressBooks(IEnumerable<string> addressBookNames)
    {
        var names = addressBookNames.ToList();
        if (names.Count == 0)
        {
            Console.WriteLine("No Address Books found.");
            return;
        }
        
        const int nameWidth = 40;
        int totalWidth = nameWidth + 2;

        Console.Clear();
        MenuManager.PrintWelcomeScreen();
        Console.WriteLine("╔" + new string('═', totalWidth) + "╗");
        Console.WriteLine("║ " + PadRight("Address Book Name", nameWidth) + " ║");
        Console.WriteLine("╠" + new string('═', totalWidth) + "╣");

        foreach (var name in names)
        {
            Console.WriteLine("║ " + PadRight(Truncate(name, nameWidth), nameWidth) + " ║");
        }
        Console.WriteLine("╚" + new string('═', totalWidth) + "╝");
        Console.WriteLine($"\nTotal Address Books: {names.Count}");
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return "";
        
        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 2) + "..";
    }

    private static string PadRight(string value, int width)
    {
        if (string.IsNullOrEmpty(value))
            return new string(' ', width);
        
        return value.PadRight(width);
    }
}
