
namespace BDDoc.Core
{
    public interface IBDDocAttrib 
    {
        //Properties

        string Key { get; }

        string Text { get; }

        int Order { get; set; }
    }
}
