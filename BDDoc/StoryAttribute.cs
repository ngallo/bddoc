using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StoryAttribute : BDDocAttribute
    {
        //Constructors
        
        public StoryAttribute(string text) : base(text) { }
    }
}
