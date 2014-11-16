
namespace BDDoc.Core.Arguments
{
    internal interface IArgumentsParser
    {
        //Indexers

        object this[string key] { get; }

        //Properties

        string ErrorMessage { get; }
    }
}
