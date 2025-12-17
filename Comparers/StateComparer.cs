using AddressBook.Models;

namespace AddressBook.Comparers;

public class StateComparer : IComparer<Contact>
{
    public int Compare(Contact? x, Contact? y)
    {
        if (x is null || y is null) return 0;
        return string.Compare(x.State, y.State, StringComparison.InvariantCultureIgnoreCase);
    }
}