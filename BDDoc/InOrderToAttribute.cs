using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InOrderToAttribute : BDDocAttribute
    {
        //Constructors

        public InOrderToAttribute(string text) : base(text) { }
    }
}
