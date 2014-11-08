using BDDoc.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest.Fakes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute(string text)
            : base(text, 2) { }
    }
}
