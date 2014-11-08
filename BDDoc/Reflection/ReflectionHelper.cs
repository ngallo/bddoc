﻿using BDDoc.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BDDoc.Reflection
{
    internal class ReflectionHelper : IReflectionHelper
    {
        //Constants

        public const string CMissingStoryAttributeAttributeExceptionMessage ="Missing StoryAttribute. Please decorate your test class with StoryAttribute.";
        public const string CMissingScenarioAttributeExceptionMessage = "Missing ScenarioAttribute. Please decorate your test method with ScenarioAttribute.";

        //Methods

        public void RetrieveStoryAttributes(out StoryInfoAttribute storyInfoAttribute, out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes)
        {
            storyAttributes = null;
            storyInfoAttribute = null;

            var skipFrames = 0;
            do
            {
                skipFrames++;
                var frame = new StackFrame(skipFrames);

                var method = frame.GetMethod();
                if (method == null)
                {
                    throw new BDDocConfigurationException(CMissingScenarioAttributeExceptionMessage);
                }

                //Get scenario's attributes
                var methodAttribs = method.GetCustomAttributes(typeof(BDDocAttribute), true);
                scenarioAttributes = (from attrib in methodAttribs
                                      where attrib is IScenarioAttrib
                                      select attrib as IScenarioAttrib).ToArray();

                if (scenarioAttributes.Count == 0) continue;
                
                var declaringType = method.DeclaringType;

                if (declaringType == null) continue;

                //Get story's attributes
                var declaringTypeAttribs = declaringType.GetCustomAttributes(typeof(BDDocAttribute), true);
                storyAttributes = (from attrib in declaringTypeAttribs
                                   where attrib is IStoryAttrib
                                   select attrib as IStoryAttrib).ToArray();

                if (!storyAttributes.Any(i => i is StoryAttribute))
                {
                    throw new BDDocConfigurationException(CMissingStoryAttributeAttributeExceptionMessage);
                }

                //Get story's info attribute
                storyInfoAttribute = declaringType.GetCustomAttributes(typeof(StoryInfoAttribute), true).FirstOrDefault() as StoryInfoAttribute;
                if (storyInfoAttribute != null) return;
                var storyId = declaringType.FullName.Replace(".", "_");
                storyInfoAttribute = new StoryInfoAttribute(storyId);

                return;

            } while (true);
        }
    }
}
