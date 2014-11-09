using System;
using System.IO;
using System.Linq;
using System.Threading;
using BDDoc.Core.Arguments;

namespace BDDoc.Core
{
    internal class HtmlDocGenerator : IHtmlDocGenerator
    {
        //Constructors

        public HtmlDocGenerator(string inputDir, string outputDir)
        {
            if ((string.IsNullOrWhiteSpace(inputDir)) || (string.IsNullOrWhiteSpace(outputDir)))
            {
                throw new ArgumentNullException();
            }
            InputDir = inputDir;
            OutputDir = outputDir;
        }

        //Properties

        public string InputDir { get; private set; }

        public string OutputDir { get; private set; }

        //Methods

        public void Generate()
        {
            var searchPattern = string.Format("*.{0}", BDDocXmlConstants.CBDDocFileExtension);
            var files = Directory.GetFiles(InputDir, searchPattern);
            if (files.Length == 0)
            {
                var logger = IoC.Resolve<ILogger>();
                var message = string.Format("No {0} files found.", BDDocXmlConstants.CBDDocFileExtension);
                logger.Info(message);
                return;
            }
            foreach (var file in files)
            {
                GenerateFile(file);
            }
        }

        public void GenerateFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException();
            }
            throw new NotImplementedException();
        }
    }
}
