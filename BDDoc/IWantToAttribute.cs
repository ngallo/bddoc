using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IWantToAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors

        public IWantToAttribute(string text) : base(text, 4) { }
    }
}
