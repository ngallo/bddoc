using BDDoc.Core;
using BDDoc.Reflection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest.Reflection
{
    [ExcludeFromCodeCoverage]
    public class ReflectionHelperTests
    {
        //Tests

        [Test]
        public void RetrieveStoryAttributes_UsingANegativeSkipFrame_AsToThrowAnException()
        {
            var reflectionHelper = new ReflectionHelper();
            StoryInfoAttribute storyInfoAttribute; 
            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            Assert.Throws<ArgumentNullException>(() => reflectionHelper.RetrieveStoryAttributes(-2, out storyInfoAttribute, out storyAttributes, out scenarioAttributes));
        }
    }
}
