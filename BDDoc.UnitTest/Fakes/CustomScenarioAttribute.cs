using BDDoc.Core;
using System;

namespace BDDoc.UnitTest.Fakes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute(string text)
            : base(text, 2) { }
    }
}
