using System;

namespace BDDoc.Core.Models
{
    internal class StoryDocument : Document
    {
        //Constructors

        public StoryDocument(string fileName, string text)
            : base(text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception();
            }
            FileName = fileName;
        }

        //Properties

        public string FileName { get; private set; }
        
    }
}
