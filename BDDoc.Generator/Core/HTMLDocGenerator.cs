using BDDoc.Core.Resources;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BDDoc.Core
{
    internal class HtmlDocGenerator : IHtmlDocGenerator
    {
        //Fields

        private readonly ILogger _logger;

        //Constructors

        public HtmlDocGenerator(string inputDir, string outputDir)
        {
            if ((string.IsNullOrWhiteSpace(inputDir)) || (string.IsNullOrWhiteSpace(outputDir)))
            {
                throw new ArgumentNullException();
            }
            InputDir = inputDir;
            OutputDir = outputDir;
            _logger = IoC.Resolve<ILogger>();
        }

        //Properties

        public string InputDir { get; private set; }

        public string OutputDir { get; private set; }

        //Methods

        private static void ValidateValue(string value, string filePath)
        {
            if (!string.IsNullOrWhiteSpace(value)) return;

            var errorMessage = string.Format("Invalid bddoc file ({0}).", filePath);
            throw new InvalidDataException(errorMessage);
        }

        private static void ValidateValue(Func<bool> func, string filePath)
        {
            if (func == null)
            {
                throw new ArgumentNullException();
            }
            if (func()) return;
            var errorMessage = string.Format("Invalid bddoc file ({0}).", filePath);
            throw new InvalidDataException(errorMessage);
        }

        protected virtual XDocument LoaDocument(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException();
            }
            return XDocument.Load(uri);
        }

        public void Generate()
        {
            var searchPattern = string.Format("*.{0}", BDDocXmlConstants.CBDDocFileExtension);
            var files = Directory.GetFiles(InputDir, searchPattern);
            if (files.Length == 0)
            {
                var message = string.Format("No {0} files found.", BDDocXmlConstants.CBDDocFileExtension);
                _logger.Info(message);
                return;
            }
            foreach (var file in files)
            {
                GenerateFile(file);
            }
        }

        public void GenerateFile(string uri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uri))
                {
                    throw new ArgumentNullException();
                }
                var xDocument = LoaDocument(uri);
                if (xDocument == null)
                {
                    var message = string.Format("File {0} is empty.", uri);
                    _logger.Info(message);
                }

                var htmlBody = new StringBuilder();

                var storyElement = BDDocXmlHelper.GetStoryElement(xDocument);
                ValidateValue(() => storyElement.Name.LocalName == BDDocXmlConstants.CStoryElement, uri);

                var storyText = storyElement.Attributes().Where((a) => a.Name == BDDocXmlConstants.CTextAttribute).Select((a) => a.Value).First();
                ValidateValue(storyText, uri);

                htmlBody.AppendLine(string.Format("<h1>{0}</h1>", storyText));
                
                var itemsElement = storyElement.Elements().ElementAt(0);
                ValidateValue(() => itemsElement.Name.LocalName == BDDocXmlConstants.CItemElementCollection, uri);
                var itemsElements = itemsElement.Elements();
                ValidateValue(() => itemsElements != null, uri);
                foreach (var xElement in itemsElements)
                {
                    var element = xElement;
                    ValidateValue(() => element.Name.LocalName == BDDocXmlConstants.CItemElement, uri);
                    var key = element.Attributes().Where((a) => a.Name == BDDocXmlConstants.CKeyAttribute).Select((a) => a.Value).First();
                    var value = element.Attributes().Where((a) => a.Name == BDDocXmlConstants.CTextAttribute).Select((a) => a.Value).First();

                    htmlBody.AppendLine(string.Format("<h2>{0}: {1}</h2>", key, value));
                }
                
                var scenarioCount = storyElement.Elements().Count();
                for (var i = 1; i < scenarioCount; i++)
                {
                    var element = storyElement.Elements().ElementAt(i);
                    ValidateValue(() => element.Name.LocalName == BDDocXmlConstants.CScenarioElement, uri);
                    var value = element.Attributes().Where((a) => a.Name == BDDocXmlConstants.CTextAttribute).Select((a) => a.Value).First();
                    htmlBody.AppendLine(string.Format("<h3>{0}</h3>", value));                    
                }

                var html = string.Format(BDDocTemplates.StoryTemplate, htmlBody);
                var htmlFileName = string.Format(@"{0}\{1}.html", OutputDir, Guid.NewGuid().ToString());

                using (var outfile = new StreamWriter(htmlFileName, true))
                {
                    outfile.Write(html);
                }
            }
            catch (Exception)
            {
                var errorMessage = string.Format("Invalid bddoc file ({0}).", uri);
                throw new InvalidDataException(errorMessage);
            }
        }
    }
}
