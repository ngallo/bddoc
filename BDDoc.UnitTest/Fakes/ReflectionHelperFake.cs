using BDDoc.Core;
using BDDoc.Reflection;
using System.Collections.Generic;

namespace BDDoc.UnitTest.Fakes
{
    public class ReflectionHelperFake : IReflectionHelper
    {
        //Fields

        private readonly IList<IStoryAttrib> _storyAttributes; 
        private readonly IList<IScenarioAttrib> _scenarioAttributes ;

        //Constructors

        public ReflectionHelperFake(IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes)
        {
            _storyAttributes = storyAttributes;
            _scenarioAttributes = scenarioAttributes;
        }

        //Methods

        public void RetrieveCallingMethodAttributes(int skipFrames, out IList<Core.IStoryAttrib> storyAttributes, out IList<Core.IScenarioAttrib> scenarioAttributes)
        {
            storyAttributes = _storyAttributes;
            scenarioAttributes = _scenarioAttributes;
        }
    }
}
