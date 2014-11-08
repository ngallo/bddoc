using System;
using BDDoc.Core;
using BDDoc.Core.Documents;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest.Core.Documents
{
    [ExcludeFromCodeCoverage]
    public class StepDocumentTests
    {
        //Tests

        [Test]
        public void CreateStepDocument_WithInvalidParameters_AnExecptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new StepDocument(ScenarioStepType.Given, null));
            Assert.Throws<ArgumentNullException>(() => new StepDocument(ScenarioStepType.Given, string.Empty));
        }

        [Test]
        public void CreateStepDocument_WithValidParameters_InstanceIsInitialized()
        {
            const string text1 = "TXT1";
            const string text2 = "TXT2";

            var scenarioStepType = ScenarioStepType.Given;
            var stepDocument = new StepDocument(scenarioStepType, text1);
            var name = Enum.GetName(typeof (ScenarioStepType), scenarioStepType);
            Assert.AreSame(name, stepDocument.StepType);
            Assert.AreEqual(text1, stepDocument.Text);

            scenarioStepType = ScenarioStepType.When;
            stepDocument = new StepDocument(scenarioStepType, text2);
            name = Enum.GetName(typeof(ScenarioStepType), scenarioStepType);
            Assert.AreEqual(name, stepDocument.StepType);
            Assert.AreEqual(text2, stepDocument.Text);

            scenarioStepType = ScenarioStepType.Then;
            stepDocument = new StepDocument(scenarioStepType, text1);
            name = Enum.GetName(typeof(ScenarioStepType), scenarioStepType);
            Assert.AreEqual(name, stepDocument.StepType);
            Assert.AreEqual(text1, stepDocument.Text);

            scenarioStepType = ScenarioStepType.And;
            stepDocument = new StepDocument(scenarioStepType, text2);
            name = Enum.GetName(typeof(ScenarioStepType), scenarioStepType);
            Assert.AreEqual(name, stepDocument.StepType);
            Assert.AreEqual(text2, stepDocument.Text);
        }
    }
}
