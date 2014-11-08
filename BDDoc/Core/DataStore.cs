using BDDoc.Core.Documents;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BDDoc.Core
{
    internal sealed class DataStore : IDataStore
    {
        //Constans

        private const string CStoryElement = "Story";
        private const string CScenarioElement = "Scenario";
        private const string CStepElementCollection = "Steps";
        private const string CStepElement = "Step";
        private const string CItemElementCollection = "Items";
        private const string CItemElement = "Item";
        private const string CKeyAttribute = "Key";
        private const string CTextAttribute = "Text";

        public const string CFileExtension = "bddoc";

        //Fields

        private readonly object _syncObj = new object();

        //Methods

        private static XElement CreateItemElement(Tuple<string, string> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(CItemElement
                , new XAttribute(CKeyAttribute, item.Item1)
                , new XAttribute(CTextAttribute, item.Item2));
        }

        private static XElement CreateStory(StoryDocument storyDocument)
        {
            if (storyDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(CStoryElement
                , new XAttribute(CTextAttribute, storyDocument.Text)
                , new XElement(CItemElementCollection, from item in storyDocument
                    select CreateItemElement(item)));
        }

        private static XElement CreateScenario(ScenarioDocument scenarioDocument)
        {
            if (scenarioDocument == null)
            {
                throw new ArgumentNullException();
            }
            return new XElement(CScenarioElement
                , new XAttribute(CTextAttribute, scenarioDocument.Text)
                , new XElement(CItemElementCollection, from item in scenarioDocument
                    select new XElement(CItemElement
                        , new XAttribute(CKeyAttribute, item.Item1)
                        , new XAttribute(CTextAttribute, item.Item2)))
                , new XElement(CStepElementCollection, from step in scenarioDocument.Steps
                    select new XElement(CStepElement
                        , new XAttribute(CKeyAttribute, step.StepType)
                        , new XAttribute(CTextAttribute, step.Text)
                        )));
        }

        private static XElement GetStoryElement(XContainer document)
        {
            if (document == null)
            {
                throw new ArgumentNullException();
            }
            var xElements = document.Elements();
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            return (from element in enumerable
                where element.Name == CStoryElement
                select element).FirstOrDefault();
        }
        
        private static XElement GetItemsElement(XContainer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }
            return element.Elements().FirstOrDefault(xElement => xElement.Name == CItemElementCollection);
        }

        public static string GetFileRelativePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }
            return string.Format("{0}.{1}", fileName, CFileExtension);
        }

        public void Save(StoryDocument storyDocument, ScenarioDocument scenarioDocument)
        {
            lock (_syncObj)
            {
                var fileRelativePath = GetFileRelativePath(storyDocument.FileName);

                XDocument document = null;
                XElement storyXElement = null;

                if (File.Exists(fileRelativePath))
                {
                    try
                    {
                        document = XDocument.Load(fileRelativePath);
                        storyXElement = GetStoryElement(document);
                        if (storyXElement != null)
                        {
                            //Sets the story text.
                            var textAElement = (from attrib in storyXElement.Attributes()
                                                where attrib.Name == CTextAttribute
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
                                                where (itemElement.Name == CItemElement) 
                                                let key = (from x in itemElement.Attributes() 
                                                           where ((x.Name.LocalName.Equals(CKeyAttribute)) && (x.Value == item.Item1)) 
                                                           select x).Any() let value = (from x in itemElement.Attributes() 
                                                                                        where ((x.Name.LocalName.Equals(CTextAttribute)) && (x.Value == item.Item2)) 
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
                    document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), storyXElement);    
                }

                var scenarioXElement = CreateScenario(scenarioDocument);
                storyXElement.Add(scenarioXElement);

                document.Save(fileRelativePath);                
            }
        }
    }
}
