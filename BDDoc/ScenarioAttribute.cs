using System;
using BDDoc.Core;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public ScenarioAttribute(string text) : base(text, 5) { }
    }
}
