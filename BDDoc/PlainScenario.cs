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

        public void Given(string text, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                text = string.Format(text, args);
            }
            AddGivenStep(text);
        }

        public void And(string text, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                text = string.Format(text, args);
            }
            AddAndStep(text);
        }

        public void When(string text, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                text = string.Format(text, args);
            }
            AddWhenStep(text);
        }

        public void Then(string text, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                text = string.Format(text, args);
            }
            AddThenStep(text);
        }

        public void Complete()
        {
            Freeze();
            Save();
        }
    }
}
