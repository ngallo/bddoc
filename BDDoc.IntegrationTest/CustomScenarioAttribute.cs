using BDDoc.Core;
using System;

namespace BDDoc.IntegrationTest
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute(string text)
            : base(text, 1) { }
    }
}
