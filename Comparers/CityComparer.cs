using AddressBook.Models;

namespace AddressBook.Comparers;

public class CityComparer : IComparer<Contact>
{
    public int Compare(Contact? x, Contact? y)
    {
        if (x is null || y is null) return 0;
        return string.Compare(x.City, y.City, StringComparison.InvariantCultureIgnoreCase);
    }
}