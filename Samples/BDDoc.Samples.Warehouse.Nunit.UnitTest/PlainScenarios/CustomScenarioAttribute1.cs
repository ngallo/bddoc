using BDDoc.Core;
using System;

namespace BDDoc.Samples.Warehouse.Nunit.UnitTest.PlainScenarios
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomScenarioAttribute1 : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute1(string text)
            : base(text, 1) { }
    }
}
