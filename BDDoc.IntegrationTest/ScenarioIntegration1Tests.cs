using System;
using BDDoc.Core;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BDDoc.IntegrationTest
{
    [ExcludeFromCodeCoverage]
    [StoryInfo(StoryId)]
    [Story(StoryText, Order = StoryOrder)]
    public class ScenarioIntegration1Tests
    {
        //Constants

        private const string StoryId = "ScenarioIntegration1";
        private const string StoryText = "This story's text is used just for testing purpose";
        private const int StoryOrder = 11;
        private const string ScenarioText = "This scenario's text is used just for testing purpose";
        private const int ScenarioOrder = 22;
        private const string GivenText = "Given text";
        private const string GivenAndText = "Given And Text";
        private const string WhenText = "When text";
        private const string WhenAndText = "When And text";
        private const string ThenText = "Then text";
        private const string ThenAndText = "Then And text";

        //Tests

        [Test]
        [Scenario(ScenarioText, Order = ScenarioOrder)]
        public void CreatePlainScenario_UsingTheObjectExtensions_ANewPlainScenarioHasToBeInstanced()
        {
            var path = BDDocXmlHelper.GetFileRelativePath(StoryId);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Delete(path);

            var scenario = this.CreateScenario();
            scenario.Given(GivenText);
            scenario.And(GivenAndText);
            scenario.When(WhenText);
            scenario.And(WhenAndText);
            scenario.Then(ThenText);
            scenario.And(ThenAndText);
            scenario.Complete();

            Assert.True(File.Exists(path));

            var xDocument = XDocument.Load(path);

            var storyElement = BDDocXmlHelper.GetStoryElement(xDocument);
            Assert.IsNotNull(storyElement);
            //Check a story element has been created
            Assert.AreEqual(BDDocXmlConstants.CStoryElement, storyElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(storyElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == StoryText));
            //A child element named "items" should have been created
            Assert.AreEqual(2, storyElement.Elements().Count());
            var itemsElement = storyElement.Elements().ElementAt(0);
            Assert.IsNotNull(itemsElement);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
            Assert.AreEqual(0, itemsElement.Elements().Count());

            //A child element named "scenarios" should have been created
            var scenariosElement = storyElement.Elements().ElementAt(1);
            Assert.IsNotNull(scenariosElement);
            //Check a story element has been created
            Assert.AreEqual(BDDocXmlConstants.CScenarioElement, scenariosElement.Name.LocalName);
            Assert.IsTrue(scenariosElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == ScenarioText));
            Assert.AreEqual(2, scenariosElement.Elements().Count());
            //A child element named "items" should have been created
            var itemsElement1 = scenariosElement.Elements().ElementAt(0);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement1.Name.LocalName);
            Assert.IsNotNull(itemsElement1);
            Assert.AreEqual(0, itemsElement1.Elements().Count());
            //A child element named "steps" should have been created
            var stepsElement = scenariosElement.Elements().ElementAt(1);
            Assert.IsNotNull(stepsElement);
            Assert.AreEqual(BDDocXmlConstants.CStepElementCollection, stepsElement.Name.LocalName);
            Assert.AreEqual(6, stepsElement.Elements().Count());

            var stepElement1 = stepsElement.Elements().ElementAt(0);
            Assert.IsNotNull(stepElement1);
            Assert.IsTrue(stepElement1.Name.LocalName.Equals(BDDocXmlConstants.CStepElement));
            Assert.IsTrue(stepElement1.Attributes().ElementAt(0).Name == BDDocXmlConstants.CKeyAttribute);
            Assert.IsTrue(stepElement1.Attributes().ElementAt(0).Value== Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.Given));
            Assert.IsTrue(stepElement1.Attributes().ElementAt(1).Name == BDDocXmlConstants.CTextAttribute);
            Assert.IsTrue(stepElement1.Attributes().ElementAt(1).Value == GivenText);

            var stepElement2 = stepsElement.Elements().ElementAt(1);
            Assert.IsNotNull(stepElement2);
            Assert.IsTrue(stepElement2.Name.LocalName.Equals(BDDocXmlConstants.CStepElement));
            Assert.IsTrue(stepElement2.Attributes().ElementAt(0).Name == BDDocXmlConstants.CKeyAttribute);
            Assert.IsTrue(stepElement2.Attributes().ElementAt(0).Value == Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.And));
            Assert.IsTrue(stepElement2.Attributes().ElementAt(1).Name == BDDocXmlConstants.CTextAttribute);
            Assert.IsTrue(stepElement2.Attributes().ElementAt(1).Value == GivenAndText);

            var stepElement3 = stepsElement.Elements().ElementAt(2);
            Assert.IsNotNull(stepElement3);
            Assert.IsTrue(stepElement3.Name.LocalName.Equals(BDDocXmlConstants.CStepElement));
            Assert.IsTrue(stepElement3.Attributes().ElementAt(0).Name == BDDocXmlConstants.CKeyAttribute);
            Assert.IsTrue(stepElement3.Attributes().ElementAt(0).Value == Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.When));
            Assert.IsTrue(stepElement3.Attributes().ElementAt(1).Name == BDDocXmlConstants.CTextAttribute);
            Assert.IsTrue(stepElement3.Attributes().ElementAt(1).Value == WhenText);

            var stepElement4 = stepsElement.Elements().ElementAt(3);
            Assert.IsNotNull(stepElement4);
            Assert.IsTrue(stepElement4.Name.LocalName.Equals(BDDocXmlConstants.CStepElement));
            Assert.IsTrue(stepElement4.Attributes().ElementAt(0).Name == BDDocXmlConstants.CKeyAttribute);
            Assert.IsTrue(stepElement4.Attributes().ElementAt(0).Value == Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.And));
            Assert.IsTrue(stepElement4.Attributes().ElementAt(1).Name == BDDocXmlConstants.CTextAttribute);
            Assert.IsTrue(stepElement4.Attributes().ElementAt(1).Value == WhenAndText);

            var stepElement5 = stepsElement.Elements().ElementAt(4);
            Assert.IsNotNull(stepElement5);
            Assert.IsTrue(stepElement5.Name.LocalName.Equals(BDDocXmlConstants.CStepElement));
            Assert.IsTrue(stepElement5.Attributes().ElementAt(0).Name == BDDocXmlConstants.CKeyAttribute);
            Assert.IsTrue(stepElement5.Attributes().ElementAt(0).Value == Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.Then));
            Assert.IsTrue(stepElement5.Attributes().ElementAt(1).Name == BDDocXmlConstants.CTextAttribute);
            Assert.IsTrue(stepElement5.Attributes().ElementAt(1).Value == ThenText);

            var stepElement6 = stepsElement.Elements().ElementAt(5);
            Assert.IsNotNull(stepElement6);
            Assert.IsTrue(stepElement6.Name.LocalName.Equals(BDDocXmlConstants.CStepElement));
            Assert.IsTrue(stepElement6.Attributes().ElementAt(0).Name == BDDocXmlConstants.CKeyAttribute);
            Assert.IsTrue(stepElement6.Attributes().ElementAt(0).Value == Enum.GetName(typeof(ScenarioStepType), ScenarioStepType.And));
            Assert.IsTrue(stepElement6.Attributes().ElementAt(1).Name == BDDocXmlConstants.CTextAttribute);
            Assert.IsTrue(stepElement6.Attributes().ElementAt(1).Value == ThenAndText);

            //Delete created file
            File.Delete(path);
        }
    }
}
