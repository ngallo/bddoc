using BDDoc.Core;
using System.Collections.Generic;

namespace BDDoc.Reflection
{
    interface IReflectionHelper
    {
        //Methods

        void RetrieveCallingMethodAttributes(int skipFrames, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes);
    }
}
