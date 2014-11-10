using System.IO;
using System.Reflection;
using System.Xml.Linq;
using BDDoc.Core;
using BDDoc.Core.Documents;
using BDDoc.UnitTest.Fakes;
using Moq;
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
            Assert.AreEqual(BDDocXmlConstants.CItemElement, xElement.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == tuple.Item1));
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == tuple.Item2));
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
            Assert.AreEqual(BDDocXmlConstants.CStoryElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == storyDoc.Text));
            //Only an child element named "items" should have been created
            Assert.AreEqual(1, xElement.Elements().Count());
            var itemsElement = xElement.Elements().ElementAt(0);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
            Assert.IsNotNull(itemsElement);
            Assert.AreEqual(0, itemsElement.Elements().Count());
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
            Assert.AreEqual(BDDocXmlConstants.CStoryElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == storyDoc.Text));
            //Only an child element named "items" should have been created
            Assert.AreEqual(1, xElement.Elements().Count());
            var itemsElement = xElement.Elements().ElementAt(0);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
            Assert.IsNotNull(itemsElement);
            Assert.AreEqual(2, itemsElement.Elements().Count());

            var item1 = itemsElement.Elements().ElementAt(0);
            //Check an item element has been created for added item (ke1 and value1)
            Assert.AreEqual(BDDocXmlConstants.CItemElement, item1.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == key1));
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == value1));

            var item2 = itemsElement.Elements().ElementAt(1);
            //Check an item element has been created for added item (ke2 and value2)
            Assert.AreEqual(BDDocXmlConstants.CItemElement, item2.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == key2));
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == value2));
        }

        [Test]
        public void DataStoreXml_CreateScenario()
        {
            Assert.Throws<ArgumentNullException>(() => DataStore.CreateScenario(null));

            const string text = "TXT";

            var scenarioDoc = new ScenarioDocument(text);
            var xElement = DataStore.CreateScenario(scenarioDoc);

            //Check a scenario element has been created
            Assert.AreEqual(BDDocXmlConstants.CScenarioElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == scenarioDoc.Text));
            //Two children element named "items" and "steps" should have been created
            Assert.AreEqual(2, xElement.Elements().Count());
            
            //Check items element
            var itemsElement = xElement.Elements().ElementAt(0);
            Assert.IsNotNull(itemsElement);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
            Assert.AreEqual(0, itemsElement.Elements().Count());

            //Check steps element
            var steps = xElement.Elements().ElementAt(1);
            Assert.IsNotNull(steps);
            Assert.AreEqual(BDDocXmlConstants.CStepElementCollection, steps.Name.LocalName);
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
            Assert.AreEqual(BDDocXmlConstants.CScenarioElement, xElement.Name.LocalName);
            //Check text attribute has been assigned
            Assert.IsTrue(xElement.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == scenarioDoc.Text));
            //Two children element named "items" and "steps" should have been created
            Assert.AreEqual(2, xElement.Elements().Count());

            //Check items element
            var itemsElement = xElement.Elements().ElementAt(0);
            Assert.IsNotNull(itemsElement);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
            Assert.AreEqual(2, itemsElement.Elements().Count());

            var item1 = itemsElement.Elements().ElementAt(0);
            //Check an item element has been created for added item (ke1 and value1)
            Assert.AreEqual(BDDocXmlConstants.CItemElement, item1.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == key1));
            Assert.IsTrue(item1.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == value1));

            var item2 = itemsElement.Elements().ElementAt(1);
            //Check an item element has been created for added item (ke2 and value2)
            Assert.AreEqual(BDDocXmlConstants.CItemElement, item2.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == key2));
            Assert.IsTrue(item2.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == value2));

            //Check steps element
            var steps = xElement.Elements().ElementAt(1);
            Assert.IsNotNull(steps);
            Assert.AreEqual(BDDocXmlConstants.CStepElementCollection, steps.Name.LocalName);
            Assert.AreEqual(2, steps.Elements().Count());

            var step1 = steps.Elements().ElementAt(0);
            //Check an step element has been created for added step (ke1 and value1)
            Assert.AreEqual(BDDocXmlConstants.CStepElement, step1.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(step1.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == scenarioStepTypeStr1));
            Assert.IsTrue(step1.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == stepValue1));

            var step2 = steps.Elements().ElementAt(1);
            //Check an step element has been created for added step (ke2 and value2)
            Assert.AreEqual(BDDocXmlConstants.CStepElement, step2.Name.LocalName);
            //Check key and value attribute has been assigned
            Assert.IsTrue(step2.Attributes().Any((a) => a.Name == BDDocXmlConstants.CKeyAttribute && a.Value == scenarioStepTypeStr2));
            Assert.IsTrue(step2.Attributes().Any((a) => a.Name == BDDocXmlConstants.CTextAttribute && a.Value == stepValue2));
        }

        [Test]
        public void DataStoreXml_GetStoryElement()
        {
            Assert.Throws<ArgumentNullException>(() => BDDocXmlHelper.GetStoryElement(null));

            const string fileName = "FILENAME";
            const string text = "TXT";

            var storyDoc = new StoryDocument(fileName, text);
            var xElement = DataStore.CreateStory(storyDoc);
            var parentElement = new XDocument(xElement);

            var storyElement = BDDocXmlHelper.GetStoryElement(parentElement);
            Assert.AreEqual(xElement, storyElement);
        }

        [Test]
        public void DataStoreXml_GetStoryElement_WithInvalidElement()
        {
            Assert.Throws<ArgumentNullException>(() => BDDocXmlHelper.GetStoryElement(null));

            const string text = "TXT";

            var storyDoc = new ScenarioDocument(text);
            var xElement = DataStore.CreateScenario(storyDoc);
            var parentElement = new XDocument(xElement);

            var storyElement = BDDocXmlHelper.GetStoryElement(parentElement);
            Assert.IsNull(storyElement);
        }

        [Test]
        public void DataStoreXml_GetItemsElement()
        {
            Assert.Throws<ArgumentNullException>(() => BDDocXmlHelper.GetItemsElement(null));

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

            var itemsElement = BDDocXmlHelper.GetItemsElement(xElement);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
        }

        [Test]
        public void DataStoreXml_GetItemsElement_WithEmptyItems()
        {
            Assert.Throws<ArgumentNullException>(() => BDDocXmlHelper.GetItemsElement(null));

            const string fileName = "FILENAME";
            const string text = "TXT";
            const string key1 = "KEY1";
            const string value1 = "VALUE1";
            const string key2 = "KEY1";
            const string value2 = "VALUE2";

            var storyDoc = new StoryDocument(fileName, text);
            var xElement = DataStore.CreateStory(storyDoc);

            var itemsElement = BDDocXmlHelper.GetItemsElement(xElement);
            Assert.AreEqual(BDDocXmlConstants.CItemElementCollection, itemsElement.Name.LocalName);
        }

        [Test]
        public void DataStoreXml_GetItemsElement_WithInvalidElement()
        {
            Assert.Throws<ArgumentNullException>(() => BDDocXmlHelper.GetItemsElement(null));

            var xElement = new XElement(new XElement("TEST"));

            var itemsElement = BDDocXmlHelper.GetItemsElement(xElement);
            Assert.IsNull(itemsElement);
        }

        [Test]
        public void GetFileRelativePath_InvalidFileName_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => BDDocXmlHelper.GetFileRelativePath(null));
        }

        [Test]
        public void GetFileRelativePath_WithValidFileName()
        {
            const string fileName = "FileName";
            var relativeFileName = BDDocXmlHelper.GetFileRelativePath(fileName);
            Assert.AreEqual(string.Format("{0}.{1}", fileName, BDDocXmlConstants.CBDDocFileExtension), relativeFileName);
        }

        [Test]
        public void CreateNewDocument_WithInvalidParameter_AnExceptionIsThronw()
        {
            var dataStore = new DataStoreFake();
            Assert.Throws<ArgumentNullException>(() => dataStore.InvokeCreateNewDocument(null));
        }

        [Test]
        public void CreateNewDocument_WithValidParameter_AnInstanceIsInitialized()
        {
            const string text = "TXT";

            var dataStore = new DataStoreFake();
            var xElement = new XElement(text);
            var document = dataStore.InvokeCreateNewDocument(xElement);
            Assert.IsNotNull(document);
            Assert.AreEqual("1.0", document.Declaration.Version);
            Assert.AreEqual("utf-8", document.Declaration.Encoding);
            Assert.AreEqual("yes", document.Declaration.Standalone);
            Assert.AreEqual(1, document.Elements().Count());
            Assert.AreEqual(xElement, document.Elements().FirstOrDefault());
            Assert.AreEqual(xElement.Name.LocalName, document.Elements().First().Name.LocalName);
        }

        [Test]
        public void CheckIfAFileExist()
        {
            var dataStore = new DataStoreFake();

            Assert.Throws<ArgumentNullException>(() => dataStore.InvokeFileExist(null));

            var exist = dataStore.InvokeFileExist(Assembly.GetExecutingAssembly().Location);
            Assert.IsTrue(exist);
        }

        [Test]
        public void SaveADocument()
        {
            var dataStore = new DataStoreFake();

            var path = Assembly.GetExecutingAssembly().Location;
            
            Assert.Throws<ArgumentNullException>(() => dataStore.InvokeSave(null, null));
            Assert.Throws<ArgumentNullException>(() => dataStore.InvokeSave(null, path));
            Assert.Throws<ArgumentNullException>(() => dataStore.InvokeSave(null, null));

            const string path2 = "tests.txt";

            if (File.Exists(path2))
            {
                File.Delete(path2);
            }

            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("TEST"));
            dataStore.InvokeSave(xDocument, path2);

            Assert.IsTrue(File.Exists(path2));
            File.Delete(path2);
        }
    }
}
