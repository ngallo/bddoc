using BDDoc.Core;
using BDDoc.Core.Documents;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest.Core.Documents
{
    [ExcludeFromCodeCoverage]
    public class ScenarioDocumentTests
    {
        //Tests

        [Test]
        public void CreateScenarioDocument_WithInvalidParameters_AnExecptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new ScenarioDocument(null));
            Assert.Throws<ArgumentNullException>(() => new ScenarioDocument(string.Empty));

            const string text = "TXT1";

            var scenarioDoc = new ScenarioDocument(text);
            
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.Given, null));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.Given, string.Empty));
            
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.When, null));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.When, string.Empty));

            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.Then, null));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.Then, string.Empty));

            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.And, null));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddStep(ScenarioStepType.And, string.Empty));

            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddItem(null, text));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddItem(string.Empty, text));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddItem(text, null));
            Assert.Throws<ArgumentNullException>(() => scenarioDoc.AddItem(text, string.Empty));
        }

        [Test]
        public void CreateScenarioDocument_WithValidParameters_InstanceIsInitialized()
        {
            const string text = "TXT";
            const string key = "KEY";
            const string value = "Value";

            var scenarioDoc = new ScenarioDocument(text);

            Assert.AreEqual(text, scenarioDoc.Text);

            for (var i = 1; i < 20; i++)
            {
                var key1 = string.Format("{0},{1}", key, i);
                var value1 = string.Format("{0},{1}", value, i);

                scenarioDoc.AddItem(key1, value1);

                Assert.IsTrue(scenarioDoc.Count() == i);

                Assert.AreEqual(key1, scenarioDoc.ElementAt(i - 1).Item1);
                Assert.AreEqual(value1, scenarioDoc.ElementAt(i - 1).Item2);
            }

            for (var i = 0; i < 20; i++)
            {
                var j = i * 4;
                var value1 = string.Format("{0},{1}", value, j);
                var value2 = string.Format("{0},{1}", value, j + 1);
                var value3 = string.Format("{0},{1}", value, j+ 2);
                var value4 = string.Format("{0},{1}", value, j+ 3);

                scenarioDoc.AddStep(ScenarioStepType.Given, value1);
                scenarioDoc.AddStep(ScenarioStepType.When, value2);
                scenarioDoc.AddStep(ScenarioStepType.Then, value3);
                scenarioDoc.AddStep(ScenarioStepType.And, value4);

                Assert.IsTrue(scenarioDoc.Steps.Count() == (i + 1) * 4);

                Assert.AreEqual(Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.Given), scenarioDoc.Steps.ElementAt(j).StepType);
                Assert.AreEqual(value1, scenarioDoc.Steps.ElementAt(j).Text);

                Assert.AreEqual(Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.When), scenarioDoc.Steps.ElementAt(j + 1).StepType);
                Assert.AreEqual(value2, scenarioDoc.Steps.ElementAt(j + 1).Text);

                Assert.AreEqual(Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.Then), scenarioDoc.Steps.ElementAt(j + 2).StepType);
                Assert.AreEqual(value3, scenarioDoc.Steps.ElementAt(j + 2).Text);

                Assert.AreEqual(Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.And), scenarioDoc.Steps.ElementAt(j + 3).StepType);
                Assert.AreEqual(value4, scenarioDoc.Steps.ElementAt(j + 3).Text);
            }
        }
    }
}
