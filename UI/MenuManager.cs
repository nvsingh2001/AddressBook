namespace AddressBook.UI;

public static class MenuManager
{
    public static void PrintWelcomeScreen()
    {
        Console.Clear();
        var banner = @"


  /$$$$$$        /$$       /$$                                              /$$$$$$$                      /$$      
 /$$__  $$      | $$      | $$                                             | $$__  $$                    | $$      
| $$  \ $$  /$$$$$$$  /$$$$$$$  /$$$$$$   /$$$$$$   /$$$$$$$ /$$$$$$$      | $$  \ $$  /$$$$$$   /$$$$$$ | $$   /$$
| $$$$$$$$ /$$__  $$ /$$__  $$ /$$__  $$ /$$__  $$ /$$_____//$$_____/      | $$$$$$$  /$$__  $$ /$$__  $$| $$  /$$/
| $$__  $$| $$  | $$| $$  | $$| $$  \__/| $$$$$$$$|  $$$$$$|  $$$$$$       | $$__  $$| $$  \ $$| $$  \ $$| $$$$$$/ 
| $$  | $$| $$  | $$| $$  | $$| $$      | $$_____/ \____  $$\____  $$      | $$  \ $$| $$  | $$| $$  | $$| $$_  $$ 
| $$  | $$|  $$$$$$$|  $$$$$$$| $$      |  $$$$$$$ /$$$$$$$//$$$$$$$/      | $$$$$$$/|  $$$$$$/|  $$$$$$/| $$ \  $$
|__/  |__/ \_______/ \_______/|__/       \_______/|_______/|_______/       |_______/  \______/  \______/ |__/  \__/
                                                                                                                   
                                                                                                                   
                                                                                                                   ";
        Console.WriteLine(banner);
    }

    public static void MainMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[A] Create a Address Book");
        Console.WriteLine("[B] Open a Address Book");
        Console.WriteLine("[C] Search Contacts");
        Console.WriteLine("[D] Get Count of Contacts");
        Console.WriteLine("[E] List All Address Books");
        Console.WriteLine("[Q] Exit");
        Console.Write("\n\nEnter your choice: ");
    }

    public static void PrintAddressBookMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("ADDRESS BOOK MENU");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Add Contact");
        Console.WriteLine("[2] Display Contacts");
        Console.WriteLine("[3] Edit Contact");
        Console.WriteLine("[4] Delete Contact");
        Console.WriteLine("[5] Sort Contacts");
        Console.WriteLine("[Q] Back");
        Console.Write("\n\nEnter your choice: ");
    }


    public static void DisplayEditMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("EDIT CONTACT");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Edit Phone Number");
        Console.WriteLine("[2] Edit Email");
        Console.WriteLine("[3] Edit Address");
        Console.WriteLine("[4] Edit City");
        Console.WriteLine("[5] Edit State");
        Console.WriteLine("[6] Edit Zip");
        Console.WriteLine("[Q] Back");
        Console.Write("\n\nEnter your choice: ");
    }

    public static void SearchContactsMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("SEARCH CONTACTS");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Search Contacts by City");
        Console.WriteLine("[2] Search Contacts by State");
        Console.WriteLine("[3] Search Contacts by City and State");
        Console.WriteLine("[Q] Back");
        Console.Write("\n\nEnter your choice: ");
    }

    public static void GetCountOfContactsMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("COUNT CONTACTS");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Get Count of Contacts by City");
        Console.WriteLine("[2] Get Count of Contacts by State");
        Console.WriteLine("[3] Get Count of Contacts by City and State");
        Console.WriteLine("[Q] Back");
        Console.Write("\n\nEnter your choice: ");
    }

    public static void DisplaySortMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("SORT CONTACTS");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Sort by Name");
        Console.WriteLine("[2] Sort by City");
        Console.WriteLine("[3] Sort by State");
        Console.WriteLine("[4] Sort by Zip");
        Console.WriteLine("[Q] Back");
        Console.Write("\n\nEnter your choice: ");
    }
}