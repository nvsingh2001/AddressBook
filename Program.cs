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
        Console.WriteLine("WELCOME TO YOUR ADDRESS BOOK");
        Console.WriteLine("------------------------------");
    }
    static void Main(string[] args)
    {
        Console.Clear();
        PrintWelcomeScreen();
        
        var contact1 = new Contact("Naman", "Singh", 7908254373, "namanvinaysingh24@gmail.com");
        Console.WriteLine(contact1);
    }
}