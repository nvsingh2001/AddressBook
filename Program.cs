using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AddressBook;

class Program
{
    static void PrintWelcomeScreen()
    {
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

    static void PrintMenu()
    {
        Console.WriteLine("WELCOME TO YOUR ADDRESS BOOK");
        Console.WriteLine("------------------------------");
        Console.WriteLine("[1] Add Contact");
        Console.WriteLine("[2] Display Contact");
        Console.WriteLine("[q] Quit");
        Console.Write("\n\nEnter your choice: ");
    }

    static Contact createContact()
    {
        string firstName = "";
        string lastName = "";
        string phoneNumber = "";
        string email = "";
        string address = "";
        string city = "";
        string state = "";
        string zip = "";
        
        var phoneRegex = new Regex(@"^\+[1-9][0-9]{7,14}$");
        var emailRegex = new Regex(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-z]{2,}$");
        
        Console.Clear();
        PrintWelcomeScreen();
        Console.WriteLine("\nAdd Contact");
        
        do
        {
            try
            {
                Console.Write("Enter first name: ");
                firstName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    throw new Exception("First name cannot be empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (string.IsNullOrWhiteSpace(firstName));
        
        do
        {
            try
            {
                Console.Write("Enter last name: ");
                lastName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    throw new Exception("Last name cannot be empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (string.IsNullOrWhiteSpace(lastName));

        do
        {
            try
            {
                Console.Write("Enter phone number: ");
                phoneNumber = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    throw new Exception("Phone number cannot be empty");
                }

                if (!phoneRegex.IsMatch(phoneNumber))
                {
                    throw new Exception("Phone number is invalid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (string.IsNullOrWhiteSpace(phoneNumber) || !phoneRegex.IsMatch(phoneNumber));

        do
        {
            try
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(email))
                {
                    break;
                }

                if (!emailRegex.IsMatch(email))
                {
                    throw new Exception("Email is invalid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }while (!emailRegex.IsMatch(email));
        
        Console.Write("Enter address: ");
        address = Console.ReadLine();
        
        Console.Write("Enter city: ");
        city = Console.ReadLine();
        
        Console.Write("Enter state: ");
        state = Console.ReadLine();
        
        Console.Write("Enter zip: ");
        zip = Console.ReadLine();
        
        return new Contact(firstName, lastName, phoneNumber, email, address, city, state, zip);
    }
    
    static void Main(string[] args)
    {
        Contact contact = null;
        do
        {
            Console.Clear();
            PrintWelcomeScreen();
            PrintMenu();
            switch(Console.ReadKey().KeyChar)
            {
                case '1':
                    contact = createContact();
                    Console.Clear();
                    PrintWelcomeScreen();
                    PrintMenu();
                    break;
                case '2':
                    Console.Clear();
                    PrintWelcomeScreen();
                    if (contact == null)
                    {
                        Console.WriteLine("Contact Not Created!");
                        Console.WriteLine("Press any key to continue . . . ");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine(contact);
                        Console.WriteLine("Press any key to continue . . . ");
                        Console.ReadKey();
                    }
                    break;
                case 'q':
                    Environment.Exit(0);
                    break;
            }
        }while(true);
    }
}