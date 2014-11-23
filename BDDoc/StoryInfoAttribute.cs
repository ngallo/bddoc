using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StoryInfoAttribute : Attribute
    {
        //Constructors

        public StoryInfoAttribute(string storyId)
        {
            if (string.IsNullOrWhiteSpace(storyId))
            {
                throw new ArgumentNullException();
            }
            StoryId = storyId;
        }

        //Properties

        public string StoryId { get; private set; }

        public string GroupName { get; set; }
    }
}
