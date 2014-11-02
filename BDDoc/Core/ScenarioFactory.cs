using BDDoc.Reflection;
using System;
using System.Collections.Generic;

namespace BDDoc.Core
{
    internal sealed class ScenarioFactory
    {
        //Fields

        private readonly IReflectionHelper _reflectionHelper;

        //Constructors

        internal ScenarioFactory(IReflectionHelper reflectionHelper)
        {
            if (reflectionHelper == null)
            {
                throw new ArgumentNullException();
            }
            _reflectionHelper = reflectionHelper;
        }

        //Methods

        public static ScenarioFactory CreateInstance()
        {
            var reflectionHelper = new ReflectionHelper();
            return new ScenarioFactory(reflectionHelper);
        }

        public PlainScenario CreateScenario(int skipFrames)
        {
            IList<IStoryAttrib> storyAttribs;
            IList<IScenarioAttrib> scenarioAttribs;
            _reflectionHelper.RetrieveCallingMethodAttributes(++skipFrames, out storyAttribs, out scenarioAttribs);
            return new PlainScenario(storyAttribs, scenarioAttribs);
        }
    }
}
