using AddressBook.Models;

namespace AddressBook.Comparers;

public class ZipComparer : IComparer<Contact>
{
    public int Compare(Contact? x, Contact? y)
    {
        if (x is null || y is null) return 0;
        return string.Compare(x.Zip, y.Zip, StringComparison.InvariantCultureIgnoreCase);
    }
}