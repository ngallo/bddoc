using BDDoc.Core;
using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class StoryAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors
        
        public StoryAttribute(string text) : base(text, 1) { }
    }
}
