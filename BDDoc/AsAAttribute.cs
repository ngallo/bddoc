using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AsAAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors

        public AsAAttribute(string text) : base(text, 3) { }
    }
}
