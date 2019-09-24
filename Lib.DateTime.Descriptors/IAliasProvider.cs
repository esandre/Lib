namespace Lib.DateTime.Descriptors
{
    public interface IAliasProvider
    {
        ITimeSlot Fetch(string name);
    }
}
