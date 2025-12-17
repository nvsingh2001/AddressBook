namespace AddressBook.UI;

public static class MenuManager
{
    public static void PrintWelcomeScreen()
    {
        Console.Clear();
        string banner = @"
  ___      _     _                    ______             _    
 / _ \    | |   | |                   | ___ \           | |   
/ /_\ \ __| | __| |_ __ ___  ___ ___  | |_/ / ___   ___ | | __
|  _  |/ _` |/ _` | '__/ _ \/ __/ __| | ___ \/ _ \ / _ \| |/ /
| | | | (_| | (_| | | |  __\/\__ \__ \ | |_/ / (_) | (_) |   < 
\_| |_/\__,_|\__,_|_|  \___||___/___/ \____/ \___/ \___/|_|\_\
";
        Console.WriteLine(banner);
    }
    
    public static void MainMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("WELCOME TO YOUR ADDRESS BOOK");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[A] Create a Address Book");
        Console.WriteLine("[B] Open a Address Book");
        Console.WriteLine("[C] Search Contacts");
        Console.WriteLine("[D] Exit");
        Console.Write("\n\nEnter your choice: ");
    }

    public static void PrintAddressBookMenu()
    {
        Console.WriteLine("[1] Add Contact");
        Console.WriteLine("[2] Display Contacts");
        Console.WriteLine("[3] Edit Contact");
        Console.WriteLine("[4] Delete Contacts");
        Console.WriteLine("[q] Quit");
        Console.Write("\n\nEnter your choice: ");
    }


    public static void DisplayEditMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("[1] Edit Phone Number");
        Console.WriteLine("[2] Edit Email");
        Console.WriteLine("[3] Edit Address");
        Console.WriteLine("[3] Edit City");
        Console.WriteLine("[4] Edit State");
        Console.WriteLine("[5] Edit Zip");
        Console.WriteLine("[6] Exit");
        Console.Write("\n\nEnter your choice: ");
    }

    public static void SearchContactsMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("[1] Search Contacts by City");
        Console.WriteLine("[2] Search Contacts by State");
        Console.WriteLine("[3] Search Contacts by City and State");
        Console.WriteLine("[q] Quit");
        Console.Write("\n\nEnter your choice: ");
    }
}
