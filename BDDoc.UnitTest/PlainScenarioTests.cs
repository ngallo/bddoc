using BDDoc.Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BDDoc.UnitTest
{
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

        private void GetAttributes(out IList<IStoryAttrib> storyAttributes, out IList<IScenarioAttrib> scenarioAttributes)
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
        public void CallingGiven_WithANotEmptyParameter_ANewStepIsAddedToTheScenario()
        {
            const string text = "TEXT";

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            var scenario = new PlainScenario(storyAttributes, scenarioAttributes);
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
    }
}
