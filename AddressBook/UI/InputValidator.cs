using System.Text.RegularExpressions;

namespace AddressBook.UI;

public static class InputValidator
{
    public static readonly Regex PhoneRegex = new(@"^\+[1-9][0-9]{7,14}$");
    public static readonly Regex EmailRegex = new(@"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-z]{2,}$");
    public static readonly Regex ZipRegex = new(@"^[1-9][0-9]{5}$");

    public static string GetValidatedInput(string prompt, Func<string, bool> validator, string errorMessage,
        bool isRequired)
    {
        return GetValidatedInput(prompt, new[] { (validator, errorMessage) }, isRequired);
    }

    public static string GetValidatedInput(string prompt,
        IEnumerable<(Func<string, bool> Validator, string ErrorMessage)> rules, bool isRequired = true)
    {
        string? input;
        do
        {
            Console.Write("\n" + prompt);
            input = Console.ReadLine();

            if (!isRequired && string.IsNullOrWhiteSpace(input)) return input ?? "";

            if (isRequired && string.IsNullOrWhiteSpace(input))
            {
                var handledByRule = false;
                foreach (var rule in rules)
                    if (!rule.Validator(input ?? ""))
                    {
                        Console.WriteLine(rule.ErrorMessage);
                        handledByRule = true;
                        break;
                    }

                if (!handledByRule) Console.WriteLine("Input cannot be empty.");
                continue;
            }

            var isValid = true;
            foreach (var rule in rules)
                if (!rule.Validator(input ?? ""))
                {
                    Console.WriteLine(rule.ErrorMessage);
                    isValid = false;
                    break;
                }

            if (isValid) return input ?? "";
        } while (true);
    }
}