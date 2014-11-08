using BDDoc.Core;
using System.Collections.Generic;

namespace BDDoc.UnitTest.Fakes
{
    public class ScenarioFake : Scenario
    {
        //Constructors

        public ScenarioFake(StoryInfoAttribute storyInfoAttribute, IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes) 
            : base(storyInfoAttribute, storyAttributes, scenarioAttributes) { }

        //Methods

        public void CallSave(bool isInvalidState, bool isFrozen)
        {
            if (isInvalidState)
            {
                SetInvalidState();
            }
            if (isFrozen)
            {
                Freeze();
            }
            Save();
        }
    }
}
