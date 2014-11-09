using System.Xml.Linq;
using BDDoc.Core;
using BDDoc.Core.Documents;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest.Core
{
    [ExcludeFromCodeCoverage]
    public class DataStoreTests
    {
        //Tests

        [Test]
        public void DataStoreXml_CreateItemElement()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateItemElement(null));

            var tuple = new Tuple<string, string>(null, null);
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateItemElement(tuple));

            const string key = "KEY";
            const string value = "VALUE";
            tuple = new Tuple<string, string>(key, value);
            
            var xElement = DataStore.CreateItemElement(tuple);

            //Check an item element has been created
            Assert.AreEqual(Constants.CDataStoreItemElement, xElement.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == tuple.Item1));
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == tuple.Item2));
        }

        [Test]
        public void DataStoreXml_CreateStory()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateStory(null));

            const string fileName = "FILENAME";
            const string text = "TXT";

            var storyDoc = new StoryDocument(fileName, text);
            var xElement = DataStore.CreateStory(storyDoc);

            //Check a story element has been created
            Assert.AreEqual(Constants.CDataStoreStoryElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == storyDoc.Text));
            //Only an child element named "items" should have been created
            Assert.AreEqual(1, xElement.Elements().Count());
            var items = xElement.Elements().ElementAt(0);
            Assert.AreEqual(Constants.CDataStoreItemElementCollection, items.Name.LocalName);
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Elements().Count());
        }

        [Test]
        public void DataStoreXml_CreateStory_AddingItems()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateStory(null));

            const string fileName = "FILENAME";
            const string text = "TXT";
            const string key1 = "KEY1";
            const string value1 = "VALUE1";
            const string key2 = "KEY1";
            const string value2 = "VALUE2";

            var storyDoc = new StoryDocument(fileName, text);
            storyDoc.AddItem(key1, value1);
            storyDoc.AddItem(key2, value2);
            var xElement = DataStore.CreateStory(storyDoc);

            //Check a story element has been created
            Assert.AreEqual(Constants.CDataStoreStoryElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == storyDoc.Text));
            //Only an child element named "items" should have been created
            Assert.AreEqual(1, xElement.Elements().Count());
            var items = xElement.Elements().ElementAt(0);
            Assert.AreEqual(Constants.CDataStoreItemElementCollection, items.Name.LocalName);
            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Elements().Count());

            var item1 = items.Elements().ElementAt(0);
            //Check an item element has been created for added item (ke1 and value1)
            Assert.AreEqual(Constants.CDataStoreItemElement, item1.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == key1));
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == value1));

            var item2 = items.Elements().ElementAt(1);
            //Check an item element has been created for added item (ke2 and value2)
            Assert.AreEqual(Constants.CDataStoreItemElement, item2.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == key2));
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == value2));
        }

        [Test]
        public void DataStoreXml_CreateScenario()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateScenario(null));

            const string text = "TXT";

            var scenarioDoc = new ScenarioDocument(text);
            var xElement = DataStore.CreateScenario(scenarioDoc);

            //Check a scenario element has been created
            Assert.AreEqual(Constants.CDataStoreScenarioElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == scenarioDoc.Text));
            //Two children element named "items" and "steps" should have been created
            Assert.AreEqual(2, xElement.Elements().Count());
            
            //Check items element
            var items = xElement.Elements().ElementAt(0);
            Assert.IsNotNull(items);
            Assert.AreEqual(Constants.CDataStoreItemElementCollection, items.Name.LocalName);
            Assert.AreEqual(0, items.Elements().Count());

            //Check steps element
            var steps = xElement.Elements().ElementAt(1);
            Assert.IsNotNull(steps);
            Assert.AreEqual(Constants.CDataStoreStepElementCollection, steps.Name.LocalName);
            Assert.AreEqual(0, steps.Elements().Count());
        }

        [Test]
        public void DataStoreXml_CreateScenario_AddingItems()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateScenario(null));

            const string text = "TXT";
            const string key1 = "KEY1";
            const string value1 = "VALUE1";
            const string key2 = "KEY1";
            const string value2 = "VALUE2";
            const ScenarioStepType scenarioStepType1 = ScenarioStepType.Given;
            const string stepValue1 = "STEPVALUE1";
            const ScenarioStepType scenarioStepType2 = ScenarioStepType.Then;
            const string stepValue2 = "STEPVALUE2";

            var scenarioStepTypeStr1 = Enum.GetName(typeof(ScenarioStepType), scenarioStepType1);
            var scenarioStepTypeStr2 = Enum.GetName(typeof(ScenarioStepType), scenarioStepType2);

            var scenarioDoc = new ScenarioDocument(text);
            scenarioDoc.AddItem(key1, value1);
            scenarioDoc.AddItem(key2, value2);
            scenarioDoc.AddStep(scenarioStepType1, stepValue1);
            scenarioDoc.AddStep(scenarioStepType2, stepValue2);
            var xElement = DataStore.CreateScenario(scenarioDoc);

            //Check a scenario element has been created
            Assert.AreEqual(Constants.CDataStoreScenarioElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == scenarioDoc.Text));
            //Two children element named "items" and "steps" should have been created
            Assert.AreEqual(2, xElement.Elements().Count());

            //Check items element
            var items = xElement.Elements().ElementAt(0);
            Assert.IsNotNull(items);
            Assert.AreEqual(Constants.CDataStoreItemElementCollection, items.Name.LocalName);
            Assert.AreEqual(2, items.Elements().Count());

            var item1 = items.Elements().ElementAt(0);
            //Check an item element has been created for added item (ke1 and value1)
            Assert.AreEqual(Constants.CDataStoreItemElement, item1.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == key1));
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == value1));

            var item2 = items.Elements().ElementAt(1);
            //Check an item element has been created for added item (ke2 and value2)
            Assert.AreEqual(Constants.CDataStoreItemElement, item2.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == key2));
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == value2));

            //Check steps element
            var steps = xElement.Elements().ElementAt(1);
            Assert.IsNotNull(steps);
            Assert.AreEqual(Constants.CDataStoreStepElementCollection, steps.Name.LocalName);
            Assert.AreEqual(2, steps.Elements().Count());

            var step1 = steps.Elements().ElementAt(0);
            //Check an step element has been created for added step (ke1 and value1)
            Assert.AreEqual(Constants.CDataStoreStepElement, step1.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(step1.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == scenarioStepTypeStr1));
            Assert.IsTrue(step1.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == stepValue1));

            var step2 = steps.Elements().ElementAt(1);
            //Check an step element has been created for added step (ke2 and value2)
            Assert.AreEqual(Constants.CDataStoreStepElement, step2.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(step2.Attributes().Any((a) => a.Name == Constants.CDataStoreKeyAttribute && a.Value == scenarioStepTypeStr2));
            Assert.IsTrue(step2.Attributes().Any((a) => a.Name == Constants.CDataStoreTextAttribute && a.Value == stepValue2));
        }

        [Test]
        public void DataStoreXml_GetStoryElement()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.GetStoryElement(null));

            const string fileName = "FILENAME";
            const string text = "TXT";

            var storyDoc = new StoryDocument(fileName, text);
            var xElement = DataStore.CreateStory(storyDoc);
            var parentElement = new XDocument(xElement);

            var storyElement = DataStore.GetStoryElement(parentElement);
            Assert.AreEqual(xElement, storyElement);
        }

        [Test]
        public void DataStoreXml_GetStoryElement_WithInvalidElement()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.GetStoryElement(null));

            const string text = "TXT";

            var storyDoc = new ScenarioDocument(text);
            var xElement = DataStore.CreateScenario(storyDoc);
            var parentElement = new XDocument(xElement);

            var storyElement = DataStore.GetStoryElement(parentElement);
            Assert.IsNull(storyElement);
        }

        [Test]
        public void DataStoreXml_GetItemsElement()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.GetItemsElement(null));

            const string fileName = "FILENAME";
            const string text = "TXT";
            const string key1 = "KEY1";
            const string value1 = "VALUE1";
            const string key2 = "KEY1";
            const string value2 = "VALUE2";

            var storyDoc = new StoryDocument(fileName, text);
            storyDoc.AddItem(key1, value1);
            storyDoc.AddItem(key2, value2);
            var xElement = DataStore.CreateStory(storyDoc);

            var itemsElement = DataStore.GetItemsElement(xElement);
            Assert.AreEqual(Constants.CDataStoreItemElementCollection, itemsElement.Name.LocalName);
        }

        [Test]
        public void DataStoreXml_GetItemsElement_WithEmptyItems()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.GetItemsElement(null));

            const string fileName = "FILENAME";
            const string text = "TXT";
            const string key1 = "KEY1";
            const string value1 = "VALUE1";
            const string key2 = "KEY1";
            const string value2 = "VALUE2";

            var storyDoc = new StoryDocument(fileName, text);
            var xElement = DataStore.CreateStory(storyDoc);

            var itemsElement = DataStore.GetItemsElement(xElement);
            Assert.AreEqual(Constants.CDataStoreItemElementCollection, itemsElement.Name.LocalName);
        }

        [Test]
        public void DataStoreXml_GetItemsElement_WithInvalidElement()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.GetItemsElement(null));

            var xElement = new XElement(new XElement("TEST"));

            var itemsElement = DataStore.GetItemsElement(xElement);
            Assert.IsNull(itemsElement);
        }

        [Test]
        public void GetFileRelativePath_InvalidFileName_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.GetFileRelativePath(null));
        }

        [Test]
        public void GetFileRelativePath_WithValidFileName()
        {
            const string fileName = "FileName";
            var relativeFileName = DataStore.GetFileRelativePath(fileName);
            Assert.AreEqual(string.Format("{0}.{1}", fileName, Constants.CDataStoreFileExtension), relativeFileName);
        }
    }
}
