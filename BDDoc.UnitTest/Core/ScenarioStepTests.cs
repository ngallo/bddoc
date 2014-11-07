using BDDoc.Core;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest.Core
{
    [ExcludeFromCodeCoverage]
    public class ScenarioStepTests
    {
        //Tests

        [Test]
        public void CreateScenarioStep_WithInvalidParameters_AnExecptionIsThrown()
        {
            Assert.Throws<ArgumentException>(() => { new ScenarioStep(ScenarioStepType.Given, -1, "TEXT"); });
            Assert.Throws<ArgumentNullException>(() => { new ScenarioStep(ScenarioStepType.Given, 1, null); });
            Assert.Throws<ArgumentNullException>(() => { new ScenarioStep(ScenarioStepType.Given, 1, string.Empty); });
        }

        [Test]
        public void CreateScenarioStep_WithValidParameters_IsInitialized()
        {
            {
                const ScenarioStepType scenarioType = ScenarioStepType.And;
                const int oder = 1;
                const string text = "TXT1";
                var scenarioStep = new ScenarioStep(scenarioType, oder, text);
                Assert.AreEqual(scenarioType, scenarioStep.StepType);
                Assert.AreEqual(oder, scenarioStep.Order);
                Assert.AreEqual(text, scenarioStep.Text);
            }
            {
                const ScenarioStepType scenarioType = ScenarioStepType.Given;
                const int oder = 2;
                const string text = "TXT2";
                var scenarioStep = new ScenarioStep(scenarioType, oder, text);
                Assert.AreEqual(scenarioType, scenarioStep.StepType);
                Assert.AreEqual(oder, scenarioStep.Order);
                Assert.AreEqual(text, scenarioStep.Text);
            }
            {
                const ScenarioStepType scenarioType = ScenarioStepType.Then;
                const int oder = 3;
                const string text = "TXT3";
                var scenarioStep = new ScenarioStep(scenarioType, oder, text);
                Assert.AreEqual(scenarioType, scenarioStep.StepType);
                Assert.AreEqual(oder, scenarioStep.Order);
                Assert.AreEqual(text, scenarioStep.Text);
            }
            {
                const ScenarioStepType scenarioType = ScenarioStepType.When;
                const int oder = 4;
                const string text = "TXT4";
                var scenarioStep = new ScenarioStep(scenarioType, oder, text);
                Assert.AreEqual(scenarioType, scenarioStep.StepType);
                Assert.AreEqual(oder, scenarioStep.Order);
                Assert.AreEqual(text, scenarioStep.Text);
            }
        }
    }
}
