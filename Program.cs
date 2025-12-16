using AddressBook.Models;
using AddressBook.Services;
using AddressBook.UI;
using AddressBook.Exceptions;

namespace AddressBook;

class Program
{
    private static readonly AddressBookService AddressBookService = new AddressBookService();

    static void EditContactField(Contact contact, int choice)
    {
        switch (choice)
        {
            case 1:
                contact.Phone = InputValidator.GetValidatedInput(
                    "Enter new phone number: ",
                    new[] {
                        ((Func<string, bool>)(input => !string.IsNullOrWhiteSpace(input)), "Phone number cannot be empty"),
                        ((Func<string, bool>)(input => InputValidator.PhoneRegex.IsMatch(input)), "Phone number is invalid")
                    },
                    isRequired: true
                );
                break;
            case 2:
                contact.Email = InputValidator.GetValidatedInput(
                    "Enter new email: ",
                    new[] {
                        ((Func<string, bool>)(input => InputValidator.EmailRegex.IsMatch(input)), "Email is invalid")
                    },
                    isRequired: false
                );
                break;
            case 3:
                Console.Write("Enter new address: ");
                contact.Address = Console.ReadLine() ?? "";
                break;
            case 4:
                Console.Write("Enter new city: ");
                contact.City = Console.ReadLine() ?? "";
                break;
            case 5:
                Console.Write("Enter new state: ");
                contact.State = Console.ReadLine() ?? "";
                break;
            case 6:
                Console.Write("Enter new zip: ");
                contact.Zip = Console.ReadLine() ?? "";
                break;
        }
        Console.WriteLine("Contact Edited Successfully!");
    }

    static Contact CollectContactInformation(ContactManager contacts)
    {
        string name = "";
        string firstName = "";
        string lastName = "";

        do
        {
            try
            {
                 firstName = InputValidator.GetValidatedInput(
                    "Enter first name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "First name cannot be empty",
                    isRequired: true
                );

                 lastName = InputValidator.GetValidatedInput(
                    "Enter last name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "Last name cannot be empty",
                    isRequired: true
                );

                name = firstName + lastName;
                if (contacts.ContainsContact(name))
                {
                    throw new DuplicateContactException(name);
                }
            }
            catch(DuplicateContactException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (contacts.ContainsContact(name));
        
        string phoneNumber = InputValidator.GetValidatedInput(
            "Enter phone number: ",
            new[] {
                ((Func<string, bool>)(input => !string.IsNullOrWhiteSpace(input)), "Phone number cannot be empty"),
                ((Func<string, bool>)(input => InputValidator.PhoneRegex.IsMatch(input)), "Phone number is invalid")
            },
            isRequired: true
        );
        
        string email = InputValidator.GetValidatedInput(
            "Enter email: ",
             new[] {
                ((Func<string, bool>)(input => InputValidator.EmailRegex.IsMatch(input)), "Email is invalid")
            },
            isRequired: false
        );
        
        Console.Write("Enter address: ");
        string address = Console.ReadLine() ?? "";
        
        Console.Write("Enter city: ");
        string city = Console.ReadLine() ?? "";
        
        Console.Write("Enter state: ");
        string state = Console.ReadLine() ?? "";
        
        Console.Write("Enter zip: ");
        string zip = Console.ReadLine() ?? "";
        
        return new Contact(firstName, lastName, phoneNumber, email, address, city, state, zip);
    }

    static void OpenAddressBook(ContactManager contacts)
    {
        char choice; 
        do
        {
            MenuManager.PrintWelcomeScreen();
            MenuManager.PrintAddressBookMenu();
            choice = Console.ReadKey().KeyChar;
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    MenuManager.PrintWelcomeScreen();
                    try
                    {
                        contacts.AddContact(CollectContactInformation(contacts));
                    }
                    catch (DuplicateContactException ex)
                    {
                        Console.WriteLine($"Error adding contact: {ex.Message}");
                    }
                    break;
                case '2':
                    Console.Clear();
                    MenuManager.PrintWelcomeScreen();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("Contact Not Created!");
                    }
                    else
                    {
                        TablePrinter.PrintContacts(contacts);
                    }
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    break;
                case '3':
                    Console.Clear();
                    MenuManager.PrintWelcomeScreen();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("No Contact to edit");
                    }
                    else
                    {
                        string firstName = InputValidator.GetValidatedInput(
                            "Enter first name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "First name cannot be empty",
                            isRequired: true
                        );

                        string lastName = InputValidator.GetValidatedInput(
                            "Enter last name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "Last name cannot be empty",
                            isRequired: true
                        );

                        if (contacts.TryGetContact(firstName + lastName, out var contact))
                        {
                            MenuManager.DisplayEditMenu();
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
                    MenuManager.PrintWelcomeScreen();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("No Contact to edit");
                    }
                    else
                    {
                        string firstName = InputValidator.GetValidatedInput(
                            "Enter first name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "First name cannot be empty",
                            isRequired: true
                        );

                        string lastName = InputValidator.GetValidatedInput(
                            "Enter last name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "Last name cannot be empty",
                            isRequired: true
                        );

                        try 
                        {
                            contacts.DeleteContact(firstName + lastName);
                            Console.WriteLine("Contact Deleted!");
                        }
                        catch (ContactNotFoundException ex)
                        {
                            Console.WriteLine(ex.Message);
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

    static void SearchContactsByCityOrState(string? city, string? state)
    {
        var searchResults = new List<Contact>();
        foreach (var contacts in AddressBookService.GetAllAddressBooks())
        {
            foreach (Contact contact in contacts)
            {
                bool match = true; 
                
                if (!string.IsNullOrEmpty(city) && !contact.City.Equals(city, StringComparison.OrdinalIgnoreCase))
                {
                    match = false;
                }
                
                if (!string.IsNullOrEmpty(state) && !contact.State.Equals(state, StringComparison.OrdinalIgnoreCase))
                {
                    match = false;
                }
                
                if (match)
                {
                    searchResults.Add(contact);
                } 
            }
        }
        TablePrinter.PrintContacts(searchResults);
    }

    static void CreateAddressBook()
    {
        while (true)
        {
            try
            {
                string addressBookName = InputValidator.GetValidatedInput(
                    "Enter address book name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "Address Book name cannot be empty.",
                    true
                );

                AddressBookService.CreateAddressBook(addressBookName);
                Console.WriteLine($"{addressBookName} created!");
                break;
            }
            catch (DuplicateAddressBookException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
    static void Main()
    {
        ContactManager contacts1 = new ContactManager();
        contacts1.AddContact(new Contact("Naman","Singh","+917908254373","nvsingh2001@hotmail.com","Matelli Bazar","Jalpaiguri", "West Bengal","735223"));
        contacts1.AddContact(new Contact("Ankit", "Kumar", "+91790825425","ankitkumar25@gmail.com","Bihar", "Bihar", "Bihar", "800001"));
        
        ContactManager contacts2 = new ContactManager();
        contacts2.AddContact(new Contact("Naman","Singh","+917908254373","nvsingh2001@hotmail.com","Matelli Bazar","Jalpaiguri", "West Bengal","735223"));
        contacts2.AddContact(new Contact("Ankit", "Kumar", "+91790825425","ankitkumar25@gmail.com","Bihar", "Bihar", "Bihar", "800001"));
        
        AddressBookService.AddAddressBook("addressbook1", contacts1);
        AddressBookService.AddAddressBook("addressbook2", contacts2);
        
        do
        {
            MenuManager.MainMenu();
            switch (Console.ReadKey().KeyChar)
            {
                case 'A':
                    MenuManager.PrintWelcomeScreen();
                    CreateAddressBook();
                    break;
                case 'B':
                    MenuManager.PrintWelcomeScreen();
                    if (AddressBookService.IsEmpty())
                    {
                        Console.WriteLine("No Address Books created!");
                    }
                    else
                    {
                         while (true) 
                         {
                            try
                            {
                                string addressBookName = InputValidator.GetValidatedInput(
                                    "Enter address book name: ",
                                    input => !string.IsNullOrWhiteSpace(input),
                                    "Address Book name cannot be empty",
                                    true
                                );
                                OpenAddressBook(AddressBookService.GetAddressBook(addressBookName));
                                break;
                            }
                            catch (AddressBookNotFoundException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                         }
                    }
                    break;
                case 'C':
                    MenuManager.SearchContactsMenu();
                    switch (Console.ReadKey().KeyChar)
                    {
                        case '1':
                            SearchContactsByCityOrState(
                                InputValidator.GetValidatedInput(
                                    "Enter city name: ",
                                    input => !string.IsNullOrWhiteSpace(input),
                                    "City name cannot be empty",
                                    true
                                ), null);
                            break;
                        case '2':
                            SearchContactsByCityOrState(null, InputValidator.GetValidatedInput(
                                "Enter State name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "State name cannot be empty",
                                true
                            ));
                            break;
                        case '3':
                            string cityName = InputValidator.GetValidatedInput(
                                "Enter city name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "City name cannot be empty",
                                true
                            );
                            string stateName = InputValidator.GetValidatedInput(
                                "Enter State name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "State name cannot be empty",
                                true
                            );
                            SearchContactsByCityOrState(cityName, stateName);
                            break;
                        
                    }
                    Console.WriteLine("\n\nPress any key to continue");
                    Console.ReadKey();
                    break;
                case 'D':
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