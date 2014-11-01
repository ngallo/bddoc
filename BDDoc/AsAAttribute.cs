using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AsAAttribute : BDDocAttribute
    {
        //Constructors

        public AsAAttribute(string text) : base(text) { }
    }
}
