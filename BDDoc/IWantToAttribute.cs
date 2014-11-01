using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IWantToAttribute : BDDocAttribute
    {
        //Constructors

        public IWantToAttribute(string text) : base(text) { }
    }
}
