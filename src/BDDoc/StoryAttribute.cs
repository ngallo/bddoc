using BDDoc.Core;
using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class StoryAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors
        
        public StoryAttribute(string text) : base(text, 1) { }
    }
}
