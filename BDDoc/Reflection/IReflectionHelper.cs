using System.Collections.Generic;
using BDDoc.Core;

namespace BDDoc.Reflection
{
    interface IReflectionHelper
    {
        //Methods

        void RetrieveCallingMethodAttributes(int skipFrames, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes);
    }
}
