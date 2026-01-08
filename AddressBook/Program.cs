using AddressBook.Comparers;
using AddressBook.Exceptions;
using AddressBook.Models;
using AddressBook.Services;
using AddressBook.Services.Interfaces;
using AddressBook.UI;
using AddressBook.Utilities.FileHandling;
using AddressBook.Utilities.FileHandling.Csv;
using AddressBook.Utilities.FileHandling.Database;
using AddressBook.Utilities.FileHandling.Json;
using Microsoft.Extensions.Configuration;

namespace AddressBook;

internal class Program
{
    private static IAddressBookService AddressBookService = null!;

    private static void EditContactField(Contact contact, int choice)
    {
        switch (choice)
        {
            case 1:
                contact.Phone = InputValidator.GetValidatedInput(
                    "Enter new phone number: ",
                    new[]
                    {
                        (input => !string.IsNullOrWhiteSpace(input), "Phone number cannot be empty"),
                        ((Func<string, bool>)(input => InputValidator.PhoneRegex.IsMatch(input)),
                            "Phone number is invalid")
                    }
                );
                break;
            case 2:
                contact.Email = InputValidator.GetValidatedInput(
                    "Enter new email: ",
                    new[]
                    {
                        ((Func<string, bool>)(input => InputValidator.EmailRegex.IsMatch(input)), "Email is invalid")
                    },
                    false
                );
                break;
            case 3:
                Console.Write("Enter new address: ");
                contact.Address = Console.ReadLine() ?? "";
                break;
            case 4:
                AddressBookService.RemoveContactByCityAndState(contact);
                contact.City = InputValidator.GetValidatedInput(
                    "Enter new city: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "City cannot be empty",
                    true
                );
                AddressBookService.AddContactByCityAndState(contact);
                break;
            case 5:
                AddressBookService.RemoveContactByCityAndState(contact);
                contact.State = InputValidator.GetValidatedInput(
                    "Enter new state: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "State cannot be empty",
                    true
                );
                AddressBookService.AddContactByCityAndState(contact);
                break;
            case 6:
                contact.Zip = InputValidator.GetValidatedInput(
                    "Enter new zip: ",
                    new[]
                    {
                        ((Func<string, bool>)(input => InputValidator.ZipRegex.IsMatch(input)), "Zip is invalid")
                    },
                    false
                );
                break;
        }

        Console.WriteLine("Contact Edited Successfully!");
    }

    private static Contact CollectContactInformation(IContactManager contacts)
    {
        var name = "";
        var firstName = "";
        var lastName = "";

        do
        {
            try
            {
                firstName = InputValidator.GetValidatedInput(
                    "Enter first name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "First name cannot be empty",
                    true
                );

                lastName = InputValidator.GetValidatedInput(
                    "Enter last name: ",
                    input => !string.IsNullOrWhiteSpace(input),
                    "Last name cannot be empty",
                    true
                );

                name = firstName + lastName;
                if (contacts.ContainsContact(name)) throw new DuplicateContactException(name);
            }
            catch (DuplicateContactException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (contacts.ContainsContact(name));

        var phoneNumber = InputValidator.GetValidatedInput(
            "Enter phone number: ",
            new[]
            {
                (input => !string.IsNullOrWhiteSpace(input), "Phone number cannot be empty"),
                ((Func<string, bool>)(input => InputValidator.PhoneRegex.IsMatch(input)), "Phone number is invalid")
            }
        );

        var email = InputValidator.GetValidatedInput(
            "Enter email: ",
            new[]
            {
                ((Func<string, bool>)(input => InputValidator.EmailRegex.IsMatch(input)), "Email is invalid")
            },
            false
        );

        Console.Write("Enter address: ");
        var address = Console.ReadLine() ?? "";

        var city = InputValidator.GetValidatedInput(
            "Enter city: ",
            input => !string.IsNullOrWhiteSpace(input),
            "City cannot be empty",
            true
        );

        var state = InputValidator.GetValidatedInput(
            "Enter state: ",
            input => !string.IsNullOrWhiteSpace(input),
            "State cannot be empty",
            true
        );

        var zip = InputValidator.GetValidatedInput(
            "Enter zip: ",
            new[]
            {
                ((Func<string, bool>)(input => InputValidator.ZipRegex.IsMatch(input)), "Zip is invalid")
            },
            false
        );

        return new Contact(firstName, lastName, phoneNumber, email, address, city, state, zip);
    }

    private static void OpenAddressBook(IContactManager contacts)
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
                        var newContact = CollectContactInformation(contacts);
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
                        Console.WriteLine("Contact Not Created!");
                    else
                        TablePrinter.PrintContacts(contacts);
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
                        var firstName = InputValidator.GetValidatedInput(
                            "Enter first name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "First name cannot be empty",
                            true
                        );

                        var lastName = InputValidator.GetValidatedInput(
                            "Enter last name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "Last name cannot be empty",
                            true
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
                        var firstName = InputValidator.GetValidatedInput(
                            "Enter first name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "First name cannot be empty",
                            true
                        );

                        var lastName = InputValidator.GetValidatedInput(
                            "Enter last name: ",
                            input => !string.IsNullOrWhiteSpace(input),
                            "Last name cannot be empty",
                            true
                        );

                        try
                        {
                            var deletedContact = contacts.DeleteContact(firstName + lastName);
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
                        var sortChoice = char.ToUpper(Console.ReadKey().KeyChar);
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

    private static void SearchContactsByCityOrState(string? city, string? state)
    {
        var searchResults = new List<Contact>();
        if (city != null && state != null)
        {
            var results = new List<Contact>();
            AddressBookService.StateDictionary.TryGetValue(state.ToLower(), out results);
            if (results != null && results.Count > 0)
                foreach (var result in results)
                    if (result.City.Equals(city, StringComparison.InvariantCultureIgnoreCase))
                        searchResults.Add(result);
        }
        else
        {
            if (city != null)
            {
                var cityResults = new List<Contact>();
                AddressBookService.CityDictionary.TryGetValue(city.ToLower(), out cityResults);
                if (cityResults != null && cityResults.Count > 0) searchResults.AddRange(cityResults);
            }

            if (state != null)
            {
                var stateResults = new List<Contact>();
                AddressBookService.StateDictionary.TryGetValue(state.ToLower(), out stateResults);
                if (stateResults != null && stateResults.Count > 0) searchResults.AddRange(stateResults);
            }
        }

        TablePrinter.PrintContacts(searchResults);
    }

    private static void CreateAddressBook()
    {
        while (true)
            try
            {
                var addressBookName = InputValidator.GetValidatedInput(
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

    private static async Task Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        AddressBookService = new AddressBookService(
            new List<IAddressBookIo>
            {
                new AddressBookIO(configuration["FilePaths:Text"]),
                new AddressBookCsvIO(configuration["FilePaths:Csv"]),
                new AddressBookJsonIo(configuration["FilePaths:Json"]),
                new AddressBookDbIo(configuration["ConnectionStrings:AddressBookDb"])
            }
        );

        await AddressBookService.LoadDataAsync();

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
                        Console.WriteLine("No Address Books created!");
                    else
                        while (true)
                            try
                            {
                                var addressBookName = InputValidator.GetValidatedInput(
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
                            var cityName = InputValidator.GetValidatedInput(
                                "Enter city name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "City name cannot be empty",
                                true
                            );
                            var stateName = InputValidator.GetValidatedInput(
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

                    if (char.ToUpper(Console.ReadKey().KeyChar) != 'Q')
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
                            var cityName = InputValidator.GetValidatedInput(
                                "Enter city name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "City name cannot be empty",
                                true
                            );
                            Console.WriteLine(
                                $"Number of Contacts: {AddressBookService.GetCountOfContactByCityAndState(cityName, null)}");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            break;
                        case '2':
                            var stateName = InputValidator.GetValidatedInput(
                                "Enter State name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "State name cannot be empty",
                                true
                            );
                            Console.WriteLine(
                                $"Number of Contacts: {AddressBookService.GetCountOfContactByCityAndState(null, stateName)}");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            break;
                        case '3':
                            var state = InputValidator.GetValidatedInput(
                                "Enter State name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "State name cannot be empty",
                                true
                            );
                            var city = InputValidator.GetValidatedInput(
                                "Enter city name: ",
                                input => !string.IsNullOrWhiteSpace(input),
                                "City name cannot be empty",
                                true
                            );
                            Console.WriteLine(
                                $"Number of Contacts: {AddressBookService.GetCountOfContactByCityAndState(city, state)}");
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
                    await AddressBookService.SaveDataAsync();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInvalid Input");
                    Console.WriteLine("Press any key to continue . . . ");
                    Console.ReadKey();
                    break; }
        } while (true);
    }
}
