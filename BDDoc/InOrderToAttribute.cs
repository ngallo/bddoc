using System;
using BDDoc.Core;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InOrderToAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors

        public InOrderToAttribute(string text) : base(text, 2) { }
    }
}
