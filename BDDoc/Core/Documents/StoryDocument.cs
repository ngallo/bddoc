using System;

namespace BDDoc.Core.Documents
{
    internal class StoryDocument : Document
    {
        //Constructors

        public StoryDocument(string fileName, string text)
            : base(text)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }
            FileName = fileName;
        }

        //Properties

        public string FileName { get; private set; }
        
    }
}
