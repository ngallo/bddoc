using System.Collections.Generic;

namespace BDDoc.Core.Reflection
{
    internal interface IReflectionHelper
    {
        //Methods

        void RetrieveStoryAttributes(out StoryInfoAttribute storyInfoAttribute
            , out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes);
    }
}
