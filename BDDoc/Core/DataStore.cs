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

        internal static XElement CreateStory(StoryDocument storyDocument)
        {
            if (storyDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(BDDocXmlConstants.CStoryElement
                , new XAttribute(BDDocXmlConstants.CTextAttribute, storyDocument.Text)
                , new XElement(BDDocXmlConstants.CItemElementCollection, from item in storyDocument
                    select CreateItemElement(item)));
        }

        internal static XElement CreateScenario(ScenarioDocument scenarioDocument)
        {
            if (scenarioDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(BDDocXmlConstants.CScenarioElement
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

        internal static XElement GetStoryElement(XContainer document)
        {
            if (document == null)
            {
                throw new ArgumentNullException();
            }
            var xElements = document.Elements();
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            return (from element in enumerable
                where element.Name ==BDDocXmlConstants.CStoryElement
                select element).FirstOrDefault();
        }
        
        internal static XElement GetItemsElement(XContainer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }
            return element.Elements().FirstOrDefault(xElement => xElement.Name == BDDocXmlConstants.CItemElementCollection);
        }

        internal static string GetFileRelativePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }
            return string.Format("{0}.{1}", fileName, BDDocXmlConstants.CBDDocFileExtension);
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
                var fileRelativePath = GetFileRelativePath(storyDocument.FileName);

                XDocument document = null;
                XElement storyXElement = null;

                if (FileExist(fileRelativePath))
                {
                    try
                    {
                        document = XDocument.Load(fileRelativePath);
                        storyXElement = GetStoryElement(document);
                        if (storyXElement != null)
                        {
                            //Sets the story text.
                            var textAElement = (from attrib in storyXElement.Attributes()
                                                where attrib.Name == BDDocXmlConstants.CTextAttribute
                                                select attrib).FirstOrDefault();
                            if (textAElement != null)
                            {
                                textAElement.Value = storyDocument.Text;
                            }

                            //Gets list of items
                            var items = GetItemsElement(storyXElement);
                            foreach (Tuple<string, string> item in storyDocument)
                            {
                                var contains = (from itemElement in items.Elements()
                                                where (itemElement.Name == BDDocXmlConstants.CItemElement) 
                                                let key = (from x in itemElement.Attributes()
                                                           where ((x.Name.LocalName.Equals(BDDocXmlConstants.CKeyAttribute)) && (x.Value == item.Item1)) 
                                                           select x).Any() let value = (from x in itemElement.Attributes()
                                                                                        where ((x.Name.LocalName.Equals(BDDocXmlConstants.CTextAttribute)) && (x.Value == item.Item2)) 
                                                                                        select x).Any() where key && value select value).Any();
                                if (!contains)
                                {
                                    //Add missing items
                                    items.Add(CreateItemElement(item));
                                }
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
