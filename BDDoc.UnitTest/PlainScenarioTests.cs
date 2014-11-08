using System.Globalization;
using BDDoc.Core;
using BDDoc.Core.Documents;
using BDDoc.UnitTest.Fakes;
using Moq;
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

        private const string StoryText = "This story's text is used just for testing purpose";
        private const string ScenarioText = "This scenario's text is used just for testing purpose";
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
            scenarioAttributes = new List<IScenarioAttrib> { new ScenarioAttribute(ScenarioTxt) { Order = ScenarioOrder } };
        }

        private static PlainScenario CreatePlainScenario()
        {
            const string storyId = "STORYID";

            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            return new PlainScenario(storyInfoAttribute, storyAttributes, scenarioAttributes);
        }

        //Tests

        [Test]
        public void CreatePlainScenario_WithInvalidParameters_AnExecptionIsThrown()
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
        public void CreatePlainScenario_WithValidParameters_InstanceIsInitialized()
        {
            const string storyId = "STORYID";
            var storyInfoAttribute = new StoryInfoAttribute(storyId);
            var storyAttrib = new StoryAttribute(StoryText) { Order = StoryOrder };
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

        [Test]
        public void CallingScenarioSteps_WithAnInvalidParameter_AnExceptionIsThrown()
        {
            var scenario = CreatePlainScenario();
            Assert.Throws<ArgumentNullException>(() => scenario.Given(string.Empty));
            Assert.Throws<ArgumentNullException>(() => scenario.Given(null));
            Assert.Throws<ArgumentNullException>(() => scenario.And(string.Empty));
            Assert.Throws<ArgumentNullException>(() => scenario.And(null));
            Assert.Throws<ArgumentNullException>(() => scenario.When(string.Empty));
            Assert.Throws<ArgumentNullException>(() => scenario.When(null));
            Assert.Throws<ArgumentNullException>(() => scenario.Then(string.Empty));
            Assert.Throws<ArgumentNullException>(() => scenario.Then(null));
        }

        [Test]
        public void CallingScenarioSteps_WithANotEmptyParameter_ANewStepIsAddedToTheScenario()
        {
            var scenario = CreatePlainScenario();

            var stepCounter = 0;
            var text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.Given(text);

            Assert.AreEqual(1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.Given, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);
            
            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.And(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);
            
            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.And(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);

            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.When(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.When, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);

            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.And(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);

            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.And(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);

            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.Then(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.Then, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);

            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.And(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);

            ++stepCounter;
            text = stepCounter.ToString(CultureInfo.InvariantCulture);
            scenario.And(text);

            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().Length);
            Assert.AreEqual(stepCounter + 1, scenario.GetAllSteps().ElementAt(stepCounter).Order);
            Assert.AreEqual(ScenarioStepType.And, scenario.GetAllSteps().ElementAt(stepCounter).StepType);
            Assert.AreEqual(text, scenario.GetAllSteps().ElementAt(stepCounter).Text);
        }


        [Test]
        public void CallingScenarioSteps_InTheWrongOrder_AnExceptionIsThrown()
        {
            const string text = "TEXT";

            var scenario = CreatePlainScenario();
            var ex = Assert.Throws<BDDocException>(() => scenario.And(text));
            var message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.And);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            ex = Assert.Throws<BDDocException>(() => scenario.When(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.When);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            ex = Assert.Throws<BDDocException>(() => scenario.Then(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Then);
            Assert.AreEqual(message, ex.Message);

            //Given

            scenario = CreatePlainScenario();
            scenario.Given(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Given);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Then(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Then);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Given);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Then(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Then);
            Assert.AreEqual(message, ex.Message);

            //When

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Given);
            Assert.AreEqual(message, ex.Message);
            
            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);

            ex = Assert.Throws<BDDocException>(() => scenario.When(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.When);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Given);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.When(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.When);
            Assert.AreEqual(message, ex.Message);

            //Then

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.Then(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Given);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.Then(text);

            ex = Assert.Throws<BDDocException>(() => scenario.When(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.When);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.Then(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Then(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Then);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.Then(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Given);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.Then(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.When(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.When);
            Assert.AreEqual(message, ex.Message);

            scenario = CreatePlainScenario();
            scenario.Given(text);
            scenario.When(text);
            scenario.Then(text);
            scenario.And(text);

            ex = Assert.Throws<BDDocException>(() => scenario.Then(text));
            message = string.Format(Constants.CExceptionMessageInvalidStep, ScenarioStepType.Then);
            Assert.AreEqual(message, ex.Message);
        }

        [Test]
        public void Scenario_MarkedAsCompleted_WithoutAttachingDataStore_AnExceptionIsThrown()
        {
            var scenario = CreatePlainScenario();
            Assert.Throws<ArgumentNullException>(() => scenario.AttachDataStore(null));
            Assert.Throws<InvalidOperationException>(scenario.Complete);
        }

        [Test]
        public void Scenario_MarkedAsCompleted_CanNotBeUpdated()
        {
            const string text = "TEXT";

            var scenario = CreatePlainScenario();
            var dataStoreMock = new Mock<IDataStore>();
            scenario.AttachDataStore(dataStoreMock.Object);

            scenario.Given(text);
            scenario.And(text);
            scenario.When(text);
            scenario.And(text);
            scenario.Then(text);
            scenario.And(text);
            scenario.Complete();

            var ex = Assert.Throws<BDDocException>(() => scenario.Given(text));
            Assert.AreEqual(Constants.CExceptionMessageFrozenScenario, ex.Message);
            ex = Assert.Throws<BDDocException>(() => scenario.And(text));
            Assert.AreEqual(Constants.CExceptionMessageFrozenScenario, ex.Message);
            ex = Assert.Throws<BDDocException>(() => scenario.When(text));
            Assert.AreEqual(Constants.CExceptionMessageFrozenScenario, ex.Message);
            ex = Assert.Throws<BDDocException>(() => scenario.Then(text));
            Assert.AreEqual(Constants.CExceptionMessageFrozenScenario, ex.Message);
            ex = Assert.Throws<BDDocException>(scenario.Complete);
            Assert.AreEqual(Constants.CExceptionMessageFrozenScenario, ex.Message);
        }

        [Test]
        public void Scenario_MarkedAsCompleted_SaveResults()
        {
            var scenario = CreatePlainScenario();
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.Setup(m => m.Save(It.IsAny<StoryDocument>(), It.IsAny<ScenarioDocument>()));
            scenario.AttachDataStore(dataStoreMock.Object);
            scenario.Complete();
            dataStoreMock.Verify(m => m.Save(It.IsAny<StoryDocument>(), It.IsAny<ScenarioDocument>()), Times.Once);
        }

        [Test]
        public void Scenario_MarkedAsCompleted_SaveResultsAndCreateDocuments()
        {
            const string storyId = "STORYID";
            const string asAText = "AsAText";
            const string iWantText = "IWantText";
            const string customScenarioText1 = "CustomText1";
            const string customScenarioText2 = "CustomText2";

            var storyInfoAttribute = new StoryInfoAttribute(storyId);
            var storyAttribute = new StoryAttribute(StoryText) { Order = StoryOrder };
            var asAAttribute = new AsAAttribute(asAText) { Order = 2 };
            var iWantAttribute = new IWantToAttribute(iWantText) { Order = 1 };
            var storyAttributes = new List<IStoryAttrib> { storyAttribute, asAAttribute, iWantAttribute };
            
            var scenarioAttrib = new ScenarioAttribute(ScenarioText) { Order = ScenarioOrder };
            var customScenarioAttribute1 = new CustomScenarioAttribute(customScenarioText1) { Order = 2 };
            var customScenarioAttribute2 = new CustomScenarioAttribute(customScenarioText2) { Order = 1 };
            var scenarioAttributes = new List<IScenarioAttrib> { scenarioAttrib, customScenarioAttribute1, customScenarioAttribute2 };

            var scenario = new PlainScenario(storyInfoAttribute, storyAttributes.ToArray(), scenarioAttributes.ToArray());
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.Setup(m => m.Save(It.IsAny<StoryDocument>(), It.IsAny<ScenarioDocument>()))
                .Callback<StoryDocument, ScenarioDocument>((storyDocument, scenarioDocument) =>
                {
                    Assert.AreEqual(StoryText, storyDocument.Text);

                    Assert.AreEqual(2, storyDocument.Count());

                    Assert.AreEqual(asAAttribute.Key, storyDocument.ElementAt(1).Item1);
                    Assert.AreEqual(asAAttribute.Text, storyDocument.ElementAt(1).Item2);
                    Assert.AreEqual(iWantAttribute.Key, storyDocument.ElementAt(0).Item1);
                    Assert.AreEqual(iWantAttribute.Text, storyDocument.ElementAt(0).Item2);
                    
                    Assert.AreEqual(ScenarioText, scenarioDocument.Text);
                    Assert.AreEqual(2, scenarioDocument.Count());
                    Assert.AreEqual(customScenarioAttribute1.Key, scenarioDocument.ElementAt(1).Item1);
                    Assert.AreEqual(customScenarioAttribute1.Text, scenarioDocument.ElementAt(1).Item2);
                    Assert.AreEqual(customScenarioAttribute2.Key, scenarioDocument.ElementAt(0).Item1);
                    Assert.AreEqual(customScenarioAttribute2.Text, scenarioDocument.ElementAt(0).Item2);
                });
            scenario.AttachDataStore(dataStoreMock.Object);
            scenario.Complete();
        }

        [Test]
        public void Scenario_MarkedAsInvalid_CanNotBeSaved()
        {
            var scenario = CreatePlainScenario();
            Assert.Throws<BDDocException>(() => scenario.Then("TEXT"));
            var ex = Assert.Throws<BDDocException>(scenario.Complete);
            Assert.AreEqual(Constants.CExceptionMessageFrozenScenario, ex.Message);
        }

        [Test]
        public void ScenarioSave_InvalidParametersAndState_ThrowAnException()
        {
            const string storyId = "STORYID";

            var storyInfoAttribute = new StoryInfoAttribute(storyId);

            IList<IStoryAttrib> storyAttributes;
            IList<IScenarioAttrib> scenarioAttributes;
            GetAttributes(out storyAttributes, out scenarioAttributes);
            
            var scenario = new ScenarioFake(storyInfoAttribute, storyAttributes, scenarioAttributes);
            var ex = Assert.Throws<BDDocException>(() => scenario.CallSave(false, false));
            Assert.AreEqual(Constants.CExceptionMessageCanNotSaveNotCompleted, ex.Message);

            scenario = new ScenarioFake(storyInfoAttribute, storyAttributes, scenarioAttributes);
            ex = Assert.Throws<BDDocException>(() => scenario.CallSave(true, false));
            Assert.AreEqual(Constants.CExceptionMessageInvalidState, ex.Message);

            scenario = new ScenarioFake(storyInfoAttribute, storyAttributes, scenarioAttributes);
            Assert.Throws<InvalidOperationException>(() => scenario.CallSave(false, true));

            scenario = new ScenarioFake(storyInfoAttribute, storyAttributes, scenarioAttributes);
            var dataStoreMock = new Mock<IDataStore>();
            scenario.AttachDataStore(dataStoreMock.Object);
            Assert.DoesNotThrow(() => scenario.CallSave(false, true));

            scenario = new ScenarioFake(storyInfoAttribute, storyAttributes, scenarioAttributes);
            ex = Assert.Throws<BDDocException>(() => scenario.CallSave(true, true));
            Assert.AreEqual(Constants.CExceptionMessageInvalidState, ex.Message);
        }
    }
}
