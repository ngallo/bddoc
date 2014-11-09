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
            return new XElement(Constants.CDataStoreItemElement
                , new XAttribute(Constants.CDataStoreKeyAttribute, item.Item1)
                , new XAttribute(Constants.CDataStoreTextAttribute, item.Item2));
        }

        internal static XElement CreateStory(StoryDocument storyDocument)
        {
            if (storyDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(Constants.CDataStoreStoryElement
                , new XAttribute(Constants.CDataStoreTextAttribute, storyDocument.Text)
                , new XElement(Constants.CDataStoreItemElementCollection, from item in storyDocument
                    select CreateItemElement(item)));
        }

        internal static XElement CreateScenario(ScenarioDocument scenarioDocument)
        {
            if (scenarioDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(Constants.CDataStoreScenarioElement
                , new XAttribute(Constants.CDataStoreTextAttribute, scenarioDocument.Text)
                , new XElement(Constants.CDataStoreItemElementCollection, from item in scenarioDocument
                    select new XElement(Constants.CDataStoreItemElement
                        , new XAttribute(Constants.CDataStoreKeyAttribute, item.Item1)
                        , new XAttribute(Constants.CDataStoreTextAttribute, item.Item2)))
                , new XElement(Constants.CDataStoreStepElementCollection, from step in scenarioDocument.Steps
                    select new XElement(Constants.CDataStoreStepElement
                        , new XAttribute(Constants.CDataStoreKeyAttribute, step.StepType)
                        , new XAttribute(Constants.CDataStoreTextAttribute, step.Text)
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
                where element.Name ==Constants.CDataStoreStoryElement
                select element).FirstOrDefault();
        }
        
        internal static XElement GetItemsElement(XContainer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }
            return element.Elements().FirstOrDefault(xElement => xElement.Name == Constants.CDataStoreItemElementCollection);
        }

        internal static string GetFileRelativePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }
            return string.Format("{0}.{1}", fileName, Constants.CDataStoreFileExtension);
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
                                                where attrib.Name == Constants.CDataStoreTextAttribute
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
                                                where (itemElement.Name == Constants.CDataStoreItemElement) 
                                                let key = (from x in itemElement.Attributes()
                                                           where ((x.Name.LocalName.Equals(Constants.CDataStoreKeyAttribute)) && (x.Value == item.Item1)) 
                                                           select x).Any() let value = (from x in itemElement.Attributes()
                                                                                        where ((x.Name.LocalName.Equals(Constants.CDataStoreTextAttribute)) && (x.Value == item.Item2)) 
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
