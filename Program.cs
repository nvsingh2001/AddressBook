using AddressBook.Models;
using AddressBook.Services;
using AddressBook.UI;
using AddressBook.Exceptions;
using AddressBook.Comparers;
using AddressBook.Services.Interfaces;

namespace AddressBook;

class Program
{
    private static readonly IAddressBookService AddressBookService = new AddressBookService();

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

    static Contact CollectContactInformation(IContactManager contacts)
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

    static void OpenAddressBook(IContactManager contacts)
    {
        char choice;
        do
        {
            MenuManager.PrintAddressBookMenu();
            choice = char.ToUpper(Console.ReadKey().KeyChar);
            switch (choice)
            {
                case '1':
                    Console.Clear();
                    MenuManager.PrintWelcomeScreen();
                    try
                    {
                        Contact newContact = CollectContactInformation(contacts);
                        contacts.AddContact(newContact);
                        AddressBookService.AddContactByCityAndState(newContact);
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
                        Console.WriteLine("No Contact to Delete");
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
                            Contact deletedContact = contacts.DeleteContact(firstName + lastName);
                            AddressBookService.RemoveContactByCityAndState(deletedContact);
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
                case '5':
                    MenuManager.DisplaySortMenu();
                    if (contacts.IsEmpty())
                    {
                        Console.WriteLine("Address Book is Empty");
                        Console.ReadKey();
                    }
                    else
                    {
                        char sortChoice = char.ToUpper(Console.ReadKey().KeyChar);
                        switch (sortChoice)
                        {
                            case '1':
                                contacts.Sort();
                                Console.WriteLine("\nSorted by Name!");
                                break;
                            case '2':
                                contacts.Sort(new CityComparer());
                                Console.WriteLine("\nSorted by City!");
                                break;
                            case '3':
                                contacts.Sort(new StateComparer());
                                Console.WriteLine("\nSorted by State!");
                                break;
                            case '4':
                                contacts.Sort(new ZipComparer());
                                Console.WriteLine("\nSorted by Zip!");
                                break;
                            case 'Q':
                                break; 
                            default:
                                Console.WriteLine("\nInvalid Option");
                                break;
                        }
                        if (sortChoice != 'Q')
                        {
                            TablePrinter.PrintContacts(contacts);
                            Console.WriteLine("Press any key to continue . . . ");
                            Console.ReadKey();
                        }
                    }
                    break;
                case 'Q':
                    break;
                default:
                    Console.WriteLine("\nInvalid Input");
                    Console.WriteLine("Press any key to continue . . . ");
                    Console.ReadKey();
                    break;
            }
        } while (choice != 'Q');
    }

    static void SearchContactsByCityOrState(string? city, string? state)
    {
        var searchResults = new List<Contact>();
        if (city != null && state != null)
        {
            var results = new  List<Contact>();
            AddressBookService.StateDictionary.TryGetValue(state.ToLower(), out results);
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    if (result.City.Equals(city, StringComparison.InvariantCultureIgnoreCase))
                    {
                        searchResults.Add(result);
                    }
                }
            }
        }
        else
        {
            if (city != null)
            {
                var cityResults = new List<Contact>();
                AddressBookService.CityDictionary.TryGetValue(city.ToLower(), out cityResults);
                if (cityResults != null && cityResults.Count > 0)
                {
                    searchResults.AddRange(cityResults);
                }
            }

            if (state != null)
            {
                var stateResults = new List<Contact>();
                AddressBookService.StateDictionary.TryGetValue(state.ToLower(), out stateResults);
                if (stateResults != null && stateResults.Count > 0)
                {
                    searchResults.AddRange(stateResults);
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
        Contact contact1 = new Contact("Naman", "Singh", "+917908254373", "nvsingh2001@hotmail.com", "Matelli Bazar",
            "Jalpaiguri", "West Bengal", "735223");
        contacts1.AddContact(contact1);
        AddressBookService.AddContactByCityAndState(contact1);
        
        Contact contact2 = new Contact("Ankit", "Kumar", "+91790825425", "ankitkumar25@gmail.com", "Bihar", "Bihar",
            "Bihar", "800001");
        contacts1.AddContact(contact2);
        AddressBookService.AddContactByCityAndState(contact2);
        
        ContactManager contacts2 = new ContactManager();
        Contact contact3 = new Contact("Naman", "Singh", "+917908254373", "nvsingh2001@hotmail.com", "Matelli Bazar",
            "Jalpaiguri", "West Bengal", "735223");
        contacts2.AddContact(contact3);
        AddressBookService.AddContactByCityAndState(contact3);
        Contact contact4 = new Contact("Ankit", "Kumar", "+91790825425", "ankitkumar25@gmail.com", "Bihar", "Bihar",
            "Bihar", "800001");
        contacts2.AddContact(contact4);
        AddressBookService.AddContactByCityAndState(contact4);
        
        AddressBookService.AddAddressBook("addressbook1", contacts1);
        AddressBookService.AddAddressBook("addressbook2", contacts2);
        
        do
        {
            MenuManager.MainMenu();
            switch (char.ToUpper(Console.ReadKey().KeyChar))
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
                    switch (char.ToUpper(Console.ReadKey().KeyChar))
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
                        case 'Q':
                            break;
                        default:
                            Console.WriteLine("\nInvalid Option");
                            Console.ReadKey();
                            break;
                    }
                    if(char.ToUpper(Console.ReadKey().KeyChar) != 'Q')
                    {
                        Console.WriteLine("\n\nPress any key to continue");
                        Console.ReadKey();
                    }
                    break;
                case 'D':
                    MenuManager.GetCountOfContactsMenu();
                    switch (char.ToUpper(Console.ReadKey().KeyChar))
                    {
                        case '1':
                            string cityName = InputValidator.GetValidatedInput(
                                "Enter city name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "City name cannot be empty",
                                true
                            );
                            Console.WriteLine($"Number of Contacts: {AddressBookService.GetCountOfContactByCityAndState(cityName,null)}");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            break;
                        case '2':
                            string stateName = InputValidator.GetValidatedInput(
                                "Enter State name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "State name cannot be empty",
                                true
                            );
                            Console.WriteLine($"Number of Contacts: {AddressBookService.GetCountOfContactByCityAndState(null,stateName)}");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            break;
                        case '3':
                            string state = InputValidator.GetValidatedInput(
                                "Enter State name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "State name cannot be empty",
                                true
                            );
                            string city = InputValidator.GetValidatedInput(
                                "Enter city name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "City name cannot be empty",
                                true
                            );
                            Console.WriteLine($"Number of Contacts: {AddressBookService.GetCountOfContactByCityAndState(city,state)}");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            break;
                         case 'Q':
                            break;
                         default:
                            Console.WriteLine("\nInvalid Option");
                            Console.ReadKey();
                            break;
                    }
                    break;
                case 'E':
                    if (AddressBookService.IsEmpty())
                    {
                        Console.WriteLine("No Address Books created!");
                        Console.ReadKey();
                    }
                    else
                    {
                        TablePrinter.PrintAddressBooks(AddressBookService.GetAddressBookNames());
                        Console.WriteLine("\n\nPress any key to continue");
                        Console.ReadKey();
                    }
                    break;
                case 'Q':
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
