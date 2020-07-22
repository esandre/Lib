namespace Lib.ACL.Rule
{
    public interface IMatcher<in TType>
    {
        bool Includes(TType concrete);
    }
}
