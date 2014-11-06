using BDDoc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BDDoc.Reflection
{
    public class ReflectionHelper : IReflectionHelper
    {
        //Methods

        public void RetrieveStoryAttributes(int skipFrames, out StoryInfoAttribute storyInfoAttribute, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes)
        {
            if (skipFrames <= -2)
            {
                throw new ArgumentNullException();
            }
    #if DEBUG
            var frame = new StackFrame(++skipFrames);
    #else
            var frame = new StackFrame(skipFrames);
    #endif
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

            //Get story's info attribute
            storyInfoAttribute = declaringType.GetCustomAttributes(typeof(StoryInfoAttribute), true).FirstOrDefault() as StoryInfoAttribute;
            if (storyInfoAttribute != null) return;
            var storyId = declaringType.AssemblyQualifiedName;
            storyInfoAttribute = new StoryInfoAttribute(storyId);
        }
    }
}
