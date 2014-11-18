using System.Globalization;
using BDDoc.Resources;
using System;
using System.IO;
using System.Linq;
using System.Web.UI;
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

        private static string BuildFooter(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentNullException();
            }
            var assemblyName = typeof(HtmlDocGenerator).Assembly.GetName();
            var assemblyNameText = string.Format("{0} {1}", assemblyName.Name, assemblyName.Version);
            html = html.Replace("{BDDocGenerator_Name}", assemblyNameText);
            html = html.Replace("{BDDocGenerator_DateTime}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            return html;
        }

        protected virtual XDocument LoaDocument(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException();
            }
            return XDocument.Load(uri);
        }

        protected virtual string GetIndexHtml()
        {
            return BDDocTemplates.index;
        }

        protected virtual string GetStoryHtml()
        {
            return BDDocTemplates.story;
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
            var indexHtml = GetIndexHtml();
            indexHtml = indexHtml.Replace("{ProjectText}", "BDDoc");

            var stringWriter = new StringWriter();
            using (var writer = new HtmlTextWriter(stringWriter))
            {
                foreach (var file in files)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    string storyText;
                    string fileName;
                    GenerateFile(file, out storyText, out fileName);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, fileName);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    if (storyText.Length > 100)
                    {
                        storyText = storyText.Remove(100, storyText.Length - 100);
                        storyText = string.Format("{0}...", storyText);
                    }
                    writer.Write(storyText);
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
            }

            indexHtml = indexHtml.Replace("{Stories}", stringWriter.ToString());
            indexHtml = BuildFooter(indexHtml);
            var indexFileName = string.Format(@"{0}\index.html", OutputDir);
            if (File.Exists(indexFileName))
            {
                File.Delete(indexFileName);
            }
            using (var outfile = new StreamWriter(indexFileName, true))
            {
                outfile.Write(indexHtml);
            }
        }

        public void GenerateFile(string uri, out string storyText, out string fileName)
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
                
                var storyElement = BDDocXmlHelper.GetStoryElement(xDocument);
                ValidateValue(() => storyElement.Name.LocalName == BDDocXmlConstants.CStoryElement, uri);

                storyText = storyElement.Attributes().Where(a => a.Name == BDDocXmlConstants.CTextAttribute).Select(a => a.Value).First();
                ValidateValue(storyText, uri);
                
                var storyHtml = GetStoryHtml();
                storyHtml = storyHtml.Replace("{StoryText}", storyText);

                var stringWriter = new StringWriter();
                using (var writer = new HtmlTextWriter(stringWriter))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.RenderBeginTag(HtmlTextWriterTag.B);
                    writer.Write("{0}", storyText);
                    writer.RenderEndTag();
                    writer.RenderEndTag();

                    var itemsElement = storyElement.Elements().ElementAt(0);
                    ValidateValue(() => itemsElement.Name.LocalName == BDDocXmlConstants.CItemElementCollection, uri);
                    var itemsElements = itemsElement.Elements();
                    ValidateValue(() => itemsElements != null, uri);
                    foreach (var xElement in itemsElements)
                    {
                        var element = xElement;
                        ValidateValue(() => element.Name.LocalName == BDDocXmlConstants.CItemElement, uri);
                        var key = element.Attributes().Where(a => a.Name == BDDocXmlConstants.CKeyAttribute).Select(a => a.Value).First();
                        ValidateValue(key, uri);
                        var value = element.Attributes().Where(a => a.Name == BDDocXmlConstants.CTextAttribute).Select(a => a.Value).First();
                        ValidateValue(value, uri);
                        writer.RenderBeginTag(HtmlTextWriterTag.P);
                        writer.RenderBeginTag(HtmlTextWriterTag.B);
                        writer.Write("{0}", key);
                        writer.RenderEndTag();
                        writer.Write("{0}", value);
                        writer.RenderEndTag();
                    }

                    var scenarioCollection = storyElement.Elements().Where(x => !x.Name.LocalName.Equals(BDDocXmlConstants.CItemElementCollection)).AsQueryable();
                    var scenarios = scenarioCollection
                        .GroupBy(x => x.Attribute(BDDocXmlConstants.CTextAttribute).Value)
                            .Select(group => new
                                {
                                    Scenarios = group.OrderByDescending(x => x.Attribute(BDDocXmlConstants.CTimeStampAttribute).Value).ToArray()
                                });
                    foreach (var scenarioElement in scenarios.Select(scenario => scenario.Scenarios.First()))
                    {
                        var element = scenarioElement;
                        ValidateValue(() => element.Name.LocalName == BDDocXmlConstants.CScenarioElement, uri);
                        var value = scenarioElement.Attributes().Where(a => a.Name == BDDocXmlConstants.CTextAttribute).Select(a => a.Value).First();
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel panel-default");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel-body");
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        //Scenario title
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.RenderBeginTag(HtmlTextWriterTag.H3);
                        writer.Write("{0}", value);
                        writer.RenderEndTag();

                        writer.RenderEndTag();

                        //Scenario body
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        writer.RenderBeginTag(HtmlTextWriterTag.P);

                        //Scenario items
                        var scenarioItemsElement = scenarioElement.Elements().ElementAt(0);
                        ValidateValue(() => scenarioItemsElement != null, uri);
                        foreach (var scenarioItemElement in scenarioItemsElement.Elements())
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.RenderBeginTag(HtmlTextWriterTag.P);

                            var scenarioItemKey = scenarioItemElement.Attributes().Where(a => a.Name == BDDocXmlConstants.CKeyAttribute).Select(a => a.Value).First();
                            ValidateValue(scenarioItemKey, uri);
                            var scenarioItemValue = scenarioItemElement.Attributes().Where(a => a.Name == BDDocXmlConstants.CTextAttribute).Select(a => a.Value).First();
                            ValidateValue(scenarioItemValue, uri);

                            writer.RenderBeginTag(HtmlTextWriterTag.B);
                            writer.Write("{0}", scenarioItemKey);
                            writer.RenderEndTag();
                            writer.Write("{0}", scenarioItemValue);

                            writer.RenderEndTag();
                            writer.RenderEndTag();
                        }

                        //Scenario steps
                        var scenarioStepsElement = scenarioElement.Elements().ElementAt(1);
                        ValidateValue(() => scenarioStepsElement != null, uri);
                        var lastStepName = string.Empty;
                        var stepCounter = -20;
                        foreach (var stepElement in scenarioStepsElement.Elements())
                        {
                            var scenarioStepKey = stepElement.Attributes().Where(a => a.Name == BDDocXmlConstants.CKeyAttribute).Select(a => a.Value).First();
                            ValidateValue(scenarioStepKey, uri);
                            var scenarioStepValue = stepElement.Attributes().Where(a => a.Name == BDDocXmlConstants.CTextAttribute).Select(a => a.Value).First();
                            ValidateValue(scenarioStepValue, uri);

                            if (string.IsNullOrWhiteSpace(lastStepName) || (!lastStepName.Equals(scenarioStepKey)))
                            {
                                lastStepName = scenarioStepKey;
                                stepCounter += 20;
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Style, String.Format("margin-left:{0}px;", stepCounter));
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);

                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "label label-default");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.Write("{0}", scenarioStepKey);
                            writer.RenderEndTag();

                            writer.AddStyleAttribute(HtmlTextWriterStyle.MarginLeft, "5px");
                            writer.RenderBeginTag(HtmlTextWriterTag.I);
                            writer.Write("{0}", scenarioStepValue);
                            writer.RenderEndTag();

                            writer.RenderEndTag();

                            writer.RenderEndTag();
                        }

                        writer.RenderEndTag();
                        writer.RenderEndTag();

                        writer.RenderEndTag();

                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }

                    storyHtml = storyHtml.Replace("{StoryBody}", stringWriter.ToString());
                }

                storyHtml = BuildFooter(storyHtml);

                fileName = string.Format(@"{0}.html", Path.GetFileNameWithoutExtension(uri));
                var fullPath = string.Format(@"{0}\{1}", OutputDir, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                using (var outfile = new StreamWriter(fullPath, true))
                {
                    outfile.Write(storyHtml);
                }
            }
            catch
            {
                var errorMessage = string.Format("Invalid bddoc file ({0}).", uri);
                throw new InvalidDataException(errorMessage);
            }
        }
    }
}
