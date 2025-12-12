namespace AddressBook;

class Program
{
    static void PrintWelcomeScreen()
    {
        string banner = @"
   _____       .___  .___                              __________               __    
  /  _  \    __| _/__| _/______   ____   ______ ______ \______   \ ____   ____ |  | __
 /  /_\  \  / __ |/ __ |\_  __ \_/ __ \ /  ___//  ___/  |    |  _//  _ \ /  _ \|  |/ /
/    |    \/ /_/ / /_/ | |  | \/\  ___/ \___ \ \___ \   |    |   (  <_> |  <_> )    < 
\____|__  /\____ \____ | |__|    \___  >____  >____  >  |______  /\____/ \____/|__|_ \
        \/      \/    \/             \/     \/     \/          \/                   \/
        ";

        Console.WriteLine(banner);
        Console.WriteLine("WELCOME TO YOUR ADDRESS BOOK");
        Console.WriteLine("------------------------------");
    }
    static void Main(string[] args)
    {
        Console.Clear();
        PrintWelcomeScreen();
    }
}