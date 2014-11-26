using BDDoc.Core;
using System;

namespace BDDoc.Samples.Warehouse.Nunit.UnitTest.PlainScenarios
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomScenarioAttribute2 : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute2(string text)
            : base(text, 2) { }
    }
}
