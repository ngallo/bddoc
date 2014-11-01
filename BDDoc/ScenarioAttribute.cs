using System;

namespace BDDoc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ScenarioAttribute : BDDocAttribute
    {
        //Constructors

        public ScenarioAttribute(string text) : base(text) { }
    }
}
