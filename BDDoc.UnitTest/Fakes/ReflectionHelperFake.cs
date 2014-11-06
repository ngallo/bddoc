using BDDoc.Core;
using BDDoc.Reflection;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest.Fakes
{
    [ExcludeFromCodeCoverage]
    public class ReflectionHelperFake : IReflectionHelper
    {
        //Fields

        private readonly StoryInfoAttribute _storyInfoAttribute; 
        private readonly IList<IStoryAttrib> _storyAttributes; 
        private readonly IList<IScenarioAttrib> _scenarioAttributes ;

        //Constructors

        public ReflectionHelperFake(StoryInfoAttribute storyInfoAttribute, IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes)
        {
            _storyInfoAttribute = storyInfoAttribute;
            _storyAttributes = storyAttributes;
            _scenarioAttributes = scenarioAttributes;
        }

        //Methods

        public void RetrieveStoryAttributes(int skipFrames, out StoryInfoAttribute storyInfoAttribute, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes)
        {
            storyInfoAttribute = _storyInfoAttribute;
            storyAttributes = _storyAttributes;
            scenarioAttributes = _scenarioAttributes;
        }
    }
}
