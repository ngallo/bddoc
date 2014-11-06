﻿using BDDoc.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class ScenarioTests
    {
        //Constants

        private const string StoryText = "This story's text is used just for testing purpose";
        private const int StoryOrder = 11;
        private const string ScenarioText = "This scenario's text is used just for testing purpose";
        private const int ScenarioOrder = 22;

        //Tests

        [Test]
        public void CreateScenario_WithInvalidParameters_AnExecptionIsThrown()
        {
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            {
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, null, null));
            }

            {
                var storyAttributes = new List<IStoryAttrib>();
                var scenarioAttributes = new List<IScenarioAttrib>();
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), null));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, null, scenarioAttributes));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), scenarioAttributes.ToArray()));
            }

            {
                var storyAttributes = new List<IStoryAttrib> { new StoryAttribute(StoryText) };
                var scenarioAttributes = new List<IScenarioAttrib>();
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes, null));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, null, scenarioAttributes));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes, scenarioAttributes.ToArray()));
            }

            {
                var storyAttributes = new List<IStoryAttrib>();
                var scenarioAttributes = new List<IScenarioAttrib> { new ScenarioAttribute(ScenarioText) };
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), null));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, null, scenarioAttributes.ToArray()));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), scenarioAttributes.ToArray()));
            }

            {
                var storyAttributes = new List<IStoryAttrib> { new StoryAttribute(StoryText) };
                var scenarioAttributes = new List<IScenarioAttrib> { new ScenarioAttribute(ScenarioText) };
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), null));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(storyInfoAttribute, null, scenarioAttributes.ToArray()));
                Assert.Throws<ArgumentNullException>(() => new PlainScenario(null, storyAttributes.ToArray(), scenarioAttributes.ToArray()));
            }
        }

        [Test]
        public void CreateScenario_WithValidParameters_InstanceIsInitialized()
        {
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);
            var storyAttrib = new StoryAttribute(StoryText) {Order = StoryOrder};
            var storyAttributes = new List<IStoryAttrib> { storyAttrib };
            var scenarioAttrib = new ScenarioAttribute(ScenarioText) { Order = ScenarioOrder };
            var scenarioAttributes = new List<IScenarioAttrib> { scenarioAttrib };

            var plainScenario = new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), scenarioAttributes.ToArray());
            Assert.AreEqual(storyInfoAttribute, plainScenario.StoryInfoAttribute);
            Assert.AreEqual(1, plainScenario.StoryAttributes.Count);
            Assert.AreEqual(StoryText, plainScenario.StoryAttributes.First().Text);
            Assert.AreEqual(StoryOrder, plainScenario.StoryAttributes.First().Order);
            Assert.AreEqual(1, plainScenario.ScenarioAttributes.Count);
            Assert.AreEqual(ScenarioText, plainScenario.ScenarioAttributes.First().Text);
            Assert.AreEqual(ScenarioOrder, plainScenario.ScenarioAttributes.First().Order);
        }
    }
}
