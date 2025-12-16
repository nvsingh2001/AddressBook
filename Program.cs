using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace AddressBook;

class Program
{
    private static readonly Dictionary<string, Contacts> AddressBooks = new Dictionary<string, Contacts>();
    static void PrintWelcomeScreen()
    {
        Console.Clear();
        string banner = @"
  ___      _     _                    ______             _    
 / _ \    | |   | |                   | ___ \           | |   
/ /_\ \ __| | __| |_ __ ___  ___ ___  | |_/ / ___   ___ | | __
|  _  |/ _` |/ _` | '__/ _ \/ __/ __| | ___ \/ _ \ / _ \| |/ /
| | | | (_| | (_| | | |  __/\__ \__ \ | |_/ / (_) | (_) |   < 
\_| |_/\__,_|\__,_|_|  \___||___/___/ \____/ \___/ \___/|_|\_\
";

        Console.WriteLine(banner);
    }

    static void PrintAddressBookMenu()
    {
        Console.WriteLine("WELCOME TO YOUR ADDRESS BOOK");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Add Contact");
        Console.WriteLine("[2] Display Contacts");
        Console.WriteLine("[3] Edit Contact");
        Console.WriteLine("[4] Delete Contacts");
        Console.WriteLine("[q] Quit");
        Console.Write("\n\nEnter your choice: ");
    }


    static void DisplayEditMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("[1] Edit First Name");
        Console.WriteLine("[2] Edit Last Name");
        Console.WriteLine("[3] Edit Phone Number");
        Console.WriteLine("[4] Edit Email");
        Console.WriteLine("[5] Edit Address");
        Console.WriteLine("[6] Edit City");
        Console.WriteLine("[7] Edit State");
        Console.WriteLine("[8] Edit Zip");
        Console.WriteLine("[9] Exit");
        Console.Write("\n\nEnter your choice: ");
    }

    static void EditContactField(Contact contact, int choice)
    {
        var phoneRegex = new Regex(@"^\+[1-9][0-9]{7,14}$");
        var emailRegex = new Regex(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-z]{2,}$");
        
        switch (choice)
        {
            case 1:
                contact.FirstName = GetValidatedInput(
                    "Enter new first name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "First name cannot be empty",
                    isRequired: true
                );
                break;
            case 2:
                contact.LastName = GetValidatedInput(
                    "Enter new last name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "Last name cannot be empty",
                    isRequired: true
                );
                break;
            case 3:
                contact.Phone = GetValidatedInput(
                    "Enter new phone number: ",
                    input => !string.IsNullOrWhiteSpace(input) && phoneRegex.IsMatch(input),
                    "Phone number cannot be empty or invalid",
                    isRequired: true
                );
                break;
            case 4:
                contact.Email = GetValidatedInput(
                    "Enter new email: ",
                    input => string.IsNullOrWhiteSpace(input) || emailRegex.IsMatch(input),
                    "Email is invalid",
                    isRequired: false
                );
                break;
            case 5:
                Console.Write("Enter new address: ");
                contact.Address = Console.ReadLine();
                break;
            case 6:
                Console.Write("Enter new city: ");
                contact.City = Console.ReadLine();
                break;
            case 7:
                Console.Write("Enter new state: ");
                contact.State = Console.ReadLine();
                break;
            case 8:
                Console.Write("Enter new zip: ");
                contact.Zip = Console.ReadLine();
                break;
        }
        Console.WriteLine("Contact Edited Successfully!");
    }

    static Contact CollectContactInformation()
    {
        var phoneRegex = new Regex(@"^\+[1-9][0-9]{7,14}$");
        var emailRegex = new Regex(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-z]{2,}$");
        
        string firstName = GetValidatedInput(
            "Enter first name: ",
            input => !string.IsNullOrWhiteSpace(input),
            "First name cannot be empty",
            isRequired: true
        );
        
        string lastName = GetValidatedInput(
            "Enter last name: ",
            input => !string.IsNullOrWhiteSpace(input),
            "Last name cannot be empty",
            isRequired: true
        );
        
        string phoneNumber = GetValidatedInput(
            "Enter phone number: ",
            input => !string.IsNullOrWhiteSpace(input) && phoneRegex.IsMatch(input),
            "Phone number cannot be empty or invalid",
            isRequired: true
        );
        
        string email = GetValidatedInput(
            "Enter email: ",
            input => string.IsNullOrWhiteSpace(input) || emailRegex.IsMatch(input),
            "Email is invalid",
            isRequired: false
        );
        
        Console.Write("Enter address: ");
        string address = Console.ReadLine();
        
        Console.Write("Enter city: ");
        string city = Console.ReadLine();
        
        Console.Write("Enter state: ");
        string state = Console.ReadLine();
        
        Console.Write("Enter zip: ");
        string zip = Console.ReadLine();
        
        return new Contact(firstName, lastName, phoneNumber, email, address, city, state, zip);
    }

    static string GetValidatedInput(string prompt, Func<string, bool> validator, string errorMessage, bool isRequired)
    {
        string input;
        do
        {
            try
            {
                Console.Write('\n'+prompt);
                input = Console.ReadLine();
                
                if (!isRequired && string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                
                if (!validator(input))
                {
                    throw new Exception(errorMessage);
                }
                
                return input;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                input = null;
            }
        } while (true);
    }

    static void OpenAddressBook(Contacts contacts)
    {
        char choice; 
        do
        {
            PrintWelcomeScreen();
            PrintAddressBookMenu();
            choice = Console.ReadKey().KeyChar;
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    PrintWelcomeScreen();
                    contacts.AddContact(CollectContactInformation());
                    break;
                case '2':
                    Console.Clear();
                    PrintWelcomeScreen();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("Contact Not Created!");
                    }
                    else
                    {
                        contacts.PrintAllContacts();
                    }
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    break;
                case '3':
                    Console.Clear();
                    PrintWelcomeScreen();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("No Contact to edit");
                    }
                    else
                    {
                        string firstName = GetValidatedInput(
                            "Enter first name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "First name cannot be empty",
                            isRequired: true
                        );

                        string lastName = GetValidatedInput(
                            "Enter last name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "Last name cannot be empty",
                            isRequired: true
                        );

                        if (contacts.TryGetContact(firstName + lastName, out var contact))
                        {
                            DisplayEditMenu();
                            EditContactField(contact, int.Parse(Console.ReadKey().KeyChar.ToString()));
                        }
                        else
                        {
                            Console.WriteLine("Contact Not found!");
                        }
                    }
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    break;
                case '4':
                    Console.Clear();
                    Console.Clear();
                    PrintWelcomeScreen();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("No Contact to edit");
                    }
                    else
                    {
                        string firstName = GetValidatedInput(
                            "Enter first name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "First name cannot be empty",
                            isRequired: true
                        );

                        string lastName = GetValidatedInput(
                            "Enter last name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "Last name cannot be empty",
                            isRequired: true
                        );

                        if (contacts.DeleteContact(firstName + lastName))
                        {
                            Console.WriteLine("Contact Deleted!");
                        }
                        else
                        {
                            Console.WriteLine("Contact Not Found!");
                        }
                    }
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    break;
                case 'q':
                    break;
                default:
                    Console.WriteLine("\nInvalid Input");
                    Console.WriteLine("Press any key to continue . . . ");
                    Console.ReadKey();
                    break;
            }
        } while (choice != 'q');
    }
    
    static void MainMenu()
    {
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("Main Menu:");
        Console.WriteLine("[A] Create a Address Book");
        Console.WriteLine("[B] Open a Address Book");
        Console.WriteLine("[C] Exit");
        Console.Write("\n\nEnter your choice: ");
    }

    static void CreateAddressBook()
    {
        string addressBookName = GetValidatedInput(
            "Enter address book name: ",
            input => !string.IsNullOrWhiteSpace(input) && !AddressBooks.ContainsKey(input),
            "Address Book name cannot be empty or Cannot be duplicate.",
            true
        );
        
        AddressBooks.Add(addressBookName, new Contacts(100));
        Console.WriteLine($"{addressBookName} created!");
    }
    static void Main(string[] args)
    {
        Contacts contacts1 = new Contacts(100);
        contacts1.AddContact(new Contact("Naman","Singh","+917908254373","nvsingh2001@hotmail.com","Matelli Bazar","Jalpaiguri", "West Bengal","735223"));
        contacts1.AddContact(new Contact("Ankit", "Kumar", "+91790825425","ankitkumar25@gmail.com","Bihar", "Bihar", "Bihar", "800001"));
        
        AddressBooks.Add("addressbook1", contacts1);
        
        do
        {
            MainMenu();
            switch (Console.ReadKey().KeyChar)
            {
                case 'A':
                    PrintWelcomeScreen();
                    CreateAddressBook();
                    break;
                case 'B':
                    PrintWelcomeScreen();
                    if (AddressBooks.Count == 0)
                    {
                        Console.WriteLine("No Address Books created!");
                    }
                    else
                    {
                        string AddressBookName = GetValidatedInput(
                            "Enter address book name: ",
                            input => !string.IsNullOrWhiteSpace(input) &&  AddressBooks.ContainsKey(input),
                            "Address Book name cannot be empty or Does Not Exists",
                            true
                        );
                        OpenAddressBook(AddressBooks[AddressBookName]);
                    }
                    break;
                case 'C':
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInvalid Input");
                    Console.WriteLine("Press any key to continue . . . ");
                    Console.ReadKey();
                    break;
            }
        }while(true);
    }
}