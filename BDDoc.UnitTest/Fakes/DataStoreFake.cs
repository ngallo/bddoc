using BDDoc.Core;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace BDDoc.UnitTest.Fakes
{
    [ExcludeFromCodeCoverage]
    internal class DataStoreFake : DataStore
    {
        //Methods

        public XDocument InvokeCreateNewDocument(XElement storyElement)
        {
            return CreateNewDocument(storyElement);
        }

        public bool InvokeFileExist(string path)
        {
            return FileExist(path);
        }

        public void InvokeSave(XDocument document, string path)
        {
            Save(document, path);
        }
    }
}
