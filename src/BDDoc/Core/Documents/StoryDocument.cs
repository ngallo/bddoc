using System;

namespace BDDoc.Core.Documents
{
    internal class StoryDocument : Document
    {
        //Constructors

        public StoryDocument(string fileName, string text)
            : this(fileName, text, null) { }

        public StoryDocument(string fileName, string text, string groupName)
            : base(text)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }
            FileName = fileName;
            GroupName = groupName;
        }

        //Properties

        public string FileName { get; private set; }

        public string GroupName { get; private set; }
        
    }
}
