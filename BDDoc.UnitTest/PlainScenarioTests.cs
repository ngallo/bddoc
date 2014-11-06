using BDDoc.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class PlainScenarioTests
    {
        //Constants

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

        //Methods

        private static void GetAttributes(out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes)
        {
            storyAttributes = new List<IStoryAttrib>
            {
                new StoryAttribute(StoryTxt) {Order = StoryOrder},
                new InOrderToAttribute(InOrderToTxt) {Order = InOrderToOrder},
                new AsAAttribute(AsAtxt) {Order = AsAtxtOrder},
                new IWantToAttribute(WantTxt) {Order = WantTxtOrder}
            };

            scenarioAttributes = new List<IScenarioAttrib> {new ScenarioAttribute(ScenarioTxt) {Order = ScenarioOrder}};
        }

        //Tests

        [Test]
        public void CallingGivenOrAnd_WithANotEmptyParameter_ANewStepIsAddedToTheScenario()
        {
            const string text = "TEXT";
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            var scenario = new PlainScenario(storyInfoAttribute, storyAttributes, scenarioAttributes);
            scenario.Given(text);

            Assert.AreEqual(1, scenario.GetAllSteps().Length);
            Assert.AreEqual(1, scenario.GetAllSteps().ElementAt(0).Order);
            Assert.AreEqual(ScenarioStepType.Given, scenario.GetAllSteps().ElementAt(0).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(0).Text);

            scenario.And(text);

            Assert.AreEqual(2, scenario.GetAllSteps().Length);
            Assert.AreEqual(2, scenario.GetAllSteps().ElementAt(1).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(1).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(1).Text);
        }

        [Test]
        public void CallingWhenOrAnd_WithANotEmptyParameter_ANewStepIsAddedToTheScenario()
        {
            const string text = "TEXT";
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            var scenario = new PlainScenario(storyInfoAttribute, storyAttributes, scenarioAttributes);
            scenario.When(text);

            Assert.AreEqual(1, scenario.GetAllSteps().Length);
            Assert.AreEqual(1, scenario.GetAllSteps().ElementAt(0).Order);
            Assert.AreEqual(ScenarioStepType.When, scenario.GetAllSteps().ElementAt(0).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(0).Text);

            scenario.And(text);

            Assert.AreEqual(2, scenario.GetAllSteps().Length);
            Assert.AreEqual(2, scenario.GetAllSteps().ElementAt(1).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(1).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(1).Text);
        }

        [Test]
        public void CallingThenOrAnd_WithANotEmptyParameter_ANewStepIsAddedToTheScenario()
        {
            const string text = "TEXT";
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            var scenario = new PlainScenario(storyInfoAttribute, storyAttributes, scenarioAttributes);
            scenario.Then(text);

            Assert.AreEqual(1, scenario.GetAllSteps().Length);
            Assert.AreEqual(1, scenario.GetAllSteps().ElementAt(0).Order);
            Assert.AreEqual(ScenarioStepType.Then, scenario.GetAllSteps().ElementAt(0).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(0).Text);

            scenario.And(text);

            Assert.AreEqual(2, scenario.GetAllSteps().Length);
            Assert.AreEqual(2, scenario.GetAllSteps().ElementAt(1).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(1).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(1).Text);
        }

        [Test]
        public void Scenario_MarkedAsCompleted_CanNotBeUpdated()
        {
            const string text = "TEXT";
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            var scenario = new PlainScenario(storyInfoAttribute, storyAttributes, scenarioAttributes);
            scenario.Given(text);
            scenario.And(text);
            scenario.When(text);
            scenario.And(text);
            scenario.Then(text);
            scenario.And(text);
            scenario.Complete();
            Assert.Throws<InvalidOperationException>(() => scenario.Given(text));
            Assert.Throws<InvalidOperationException>(() => scenario.And(text));
            Assert.Throws<InvalidOperationException>(() => scenario.When(text));
            Assert.Throws<InvalidOperationException>(() => scenario.Then(text));
            Assert.Throws<InvalidOperationException>(scenario.Complete);
        }
    }
}
