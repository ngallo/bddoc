using BDDoc.Core;
using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InOrderToAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors

        public InOrderToAttribute(string text) : base(text, 2) { }
    }
}
