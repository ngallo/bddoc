using BDDoc.Core;
using System.Collections.Generic;

namespace BDDoc
{
    public sealed class PlainScenario : Scenario
    {
        //Constructors

        internal PlainScenario(StoryInfoAttribute storyInfoAttribute, IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes)
            : base(storyInfoAttribute, storyAttributes, scenarioAttributes) { }

        //Methods

        public void Given(string text)
        {
            AddGivenStep(text);
        }

        public void And(string text)
        {
            AddAndStep(text);
        }

        public void When(string text)
        {
            AddWhenStep(text);
        }

        public void Then(string text)
        {
            AddThenStep(text);
        }

        public void Complete()
        {
            Freeze();
            Save();
        }
    }
}
