using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BDDoc.Core
{
    public abstract class Scenario
    {
        //Constructors

        protected Scenario(IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes)
        {
            if (storyAttributes == null || (storyAttributes.Count == 0) 
                || (scenarioAttributes == null) || (scenarioAttributes.Count == 0))
            {
                throw new ArgumentNullException();
            }
            StoryAttributes = new ReadOnlyCollection<IStoryAttrib>(storyAttributes);
            ScenarioAttributes = new ReadOnlyCollection<IScenarioAttrib>(scenarioAttributes);
        }

        //Properties

        public ReadOnlyCollection<IStoryAttrib> StoryAttributes { get; private set; }

        public ReadOnlyCollection<IScenarioAttrib> ScenarioAttributes { get; private set; }
    }
}
