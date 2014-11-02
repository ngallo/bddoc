using BDDoc.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BDDoc.Reflection
{
    public class ReflectionHelper : IReflectionHelper
    {
        //Methods

        public void RetrieveCallingMethodAttributes(int skipFrames, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes)
        {
            var frame = new StackFrame(++skipFrames);
            var method = frame.GetMethod();

            //Get scenario's attributes
            var methodAttribs = method.GetCustomAttributes(typeof(BDDocAttribute), true);
            scenarioAttributes = (from attrib in methodAttribs
                                    where attrib is IScenarioAttrib
                                        select attrib as IScenarioAttrib).ToArray();

            //Get story's attributes
            var declaringType = method.DeclaringType;
            var declaringTypeAttribs = declaringType.GetCustomAttributes(typeof(BDDocAttribute), true);
            storyAttributes = (from attrib in declaringTypeAttribs
                                where attrib is IStoryAttrib
                                    select attrib as IStoryAttrib).ToArray();
        }
    }
}
