using BDDoc.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.IntegrationTest
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute(string text)
            : base(text, 1) { }
    }
}
