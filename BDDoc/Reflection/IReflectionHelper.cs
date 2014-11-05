using BDDoc.Core;
using System.Collections.Generic;

namespace BDDoc.Reflection
{
    interface IReflectionHelper
    {
        //Methods

        void RetrieveStoryAttributes(int skipFrames, out StoryInfoAttribute storyInfoAttribute, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes);
    }
}
