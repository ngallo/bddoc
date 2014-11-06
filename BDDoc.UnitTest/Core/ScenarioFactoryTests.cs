using System;
using System.Collections.Generic;
using BDDoc.Core;
using BDDoc.Reflection;
using Moq;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest.Core
{
    [ExcludeFromCodeCoverage]
    public class ScenarioFactoryTests
    {
        //Constants

        private const string StoryId = "STORYID";
        private const string StoryTxt = "STORY";
        private const int StoryOrder = 11;
        private const string InOrderToTxt = "INORDERTO";
        private const int InOrderToOrder = 12;
        private const string AsAtxt = "ASA";
        private const int AsAtxtOrder = 13;
        private const string WantTxt = "IWANTTO";
        private const int WantTxtOrder = 14;
        private const string ScenarioTxt = "SCENARIO";
        private const int ScenarioOrder = 15;

        //Tests

        [Test]
        public void CreateScenarioFactory_WithInvalidParameters_AnExecptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new ScenarioFactory(null));
        }

        [Test]
        public void CreateScenario_WhenReflectionHelperReturnsInvalidValue_AnExceptionIsThrown()
        {
            const int skipFrame = 11;
            var reflectionHelperMock = new Mock<IReflectionHelper>();
            StoryInfoAttribute storyInfoAttribute;
            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            reflectionHelperMock.Setup((m) => m.RetrieveStoryAttributes(skipFrame + 1, out storyInfoAttribute, out storyAttributes, out scenarioAttributes));
            var plainScenario = new ScenarioFactory(reflectionHelperMock.Object);
            Assert.Throws<ArgumentNullException>(() => plainScenario.CreateScenario(skipFrame));
            reflectionHelperMock.Verify((m) => m.RetrieveStoryAttributes(skipFrame + 1, out storyInfoAttribute, out storyAttributes, out scenarioAttributes), Times.Once);
        }
    }
}
