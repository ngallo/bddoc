using BDDoc.Core.Documents;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BDDoc.Core
{
    internal class DataStore : IDataStore
    {
        //Fields

        private const string CurrentXmlVersion = "1.0";
        private readonly object _syncObj = new object();

        //Methods

        internal static XElement CreateItemElement(Tuple<string, string> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(BDDocXmlConstants.CItemElement
                , new XAttribute(BDDocXmlConstants.CKeyAttribute, item.Item1)
                , new XAttribute(BDDocXmlConstants.CTextAttribute, item.Item2));
        }

        internal static void SetStoryAttributes(XElement storyElement, StoryDocument storyDocument)
        {
            if ((storyElement == null) || (storyDocument == null))
            {
                throw new ArgumentNullException();
            }
            storyElement.SetAttributeValue(BDDocXmlConstants.CVersionAttribute, CurrentXmlVersion);
            if (!string.IsNullOrWhiteSpace(storyDocument.GroupName))
            {
                storyElement.SetAttributeValue(BDDocXmlConstants.CGroupNameAttribute, storyDocument.GroupName);
            }
            storyElement.SetAttributeValue(BDDocXmlConstants.CTextAttribute, storyDocument.Text);
        }

        internal static XElement CreateStory(StoryDocument storyDocument)
        {
            if (storyDocument == null)
            {
                throw new ArgumentNullException();
            }
            var xElement = new XElement(BDDocXmlConstants.CStoryElement
                    , new XElement(BDDocXmlConstants.CItemElementCollection, from item in storyDocument
                                                                             select CreateItemElement(item)));
            SetStoryAttributes(xElement, storyDocument);
            return xElement;
        }

        internal static XElement CreateScenario(ScenarioDocument scenarioDocument)
        {
            if (scenarioDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(BDDocXmlConstants.CScenarioElement
                , new XAttribute(BDDocXmlConstants.CTimeStampAttribute, DateTime.UtcNow.ToString())
                , new XAttribute(BDDocXmlConstants.CTextAttribute, scenarioDocument.Text)
                , new XElement(BDDocXmlConstants.CItemElementCollection, from item in scenarioDocument
                                                                         select new XElement(BDDocXmlConstants.CItemElement
                                                                             , new XAttribute(BDDocXmlConstants.CKeyAttribute, item.Item1)
                                                                             , new XAttribute(BDDocXmlConstants.CTextAttribute, item.Item2)))
                , new XElement(BDDocXmlConstants.CStepElementCollection, from step in scenarioDocument.Steps
                                                                         select new XElement(BDDocXmlConstants.CStepElement
                                                                             , new XAttribute(BDDocXmlConstants.CKeyAttribute, step.StepType)
                                                                             , new XAttribute(BDDocXmlConstants.CTextAttribute, step.Text)
                                                                             )));
        }

        protected XDocument CreateNewDocument(XElement storyElement)
        {
            if (storyElement == null)
            {
                throw new ArgumentNullException();
            }
            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), storyElement);
        }

        protected bool FileExist(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }
            return File.Exists(path);
        }

        protected void Save(XDocument document, string path)
        {
            if ((document == null) || (string.IsNullOrEmpty(path)))
            {
                throw new ArgumentNullException();
            }
            document.Save(path);    
        }

        public void Save(StoryDocument storyDocument, ScenarioDocument scenarioDocument)
        {
            lock (_syncObj)
            {
                var fileRelativePath = BDDocXmlHelper.GetFileRelativePath(storyDocument.FileName);

                XDocument document = null;
                XElement storyXElement = null;

                if (FileExist(fileRelativePath))
                {
                    try
                    {
                        document = XDocument.Load(fileRelativePath);
                        storyXElement = BDDocXmlHelper.GetStoryElement(document);
                        if (storyXElement != null)
                        {
                            //Set story's attributes.
                            SetStoryAttributes(storyXElement, storyDocument);

                            //Gets list of items
                            var items = BDDocXmlHelper.GetItemsElement(storyXElement);
                            items.RemoveAll();
                            foreach (Tuple<string, string> item in storyDocument)
                            {
                                items.Add(CreateItemElement(item));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        document = null;
                    }
                }
                if ((document == null) || (storyXElement == null))
                {
                    storyXElement = CreateStory(storyDocument);
                    document = CreateNewDocument(storyXElement);
                }

                var scenarioXElement = CreateScenario(scenarioDocument);
                storyXElement.Add(scenarioXElement);

                Save(document, fileRelativePath);
            }
        }
    }
}
