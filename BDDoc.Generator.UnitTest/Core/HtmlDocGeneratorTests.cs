using BDDoc.Core;
using BDDoc.Core.Arguments;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Generator.UnitTest.Core
{
    [ExcludeFromCodeCoverage]
    public class HtmlDocGeneratorTests
    {
        //Tests

        [Test]
        public void InitializingHtmlDocGenerator_WithInvalidParameters_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new HtmlDocGenerator(null));
        }

        [Test]
        public void InitializingHtmlDocGenerator_WithValidParameters_HtmlDocGeneratorIsInitialized()
        {
            Program.Initialize();

            var arguments = new List<string> { "-inputdir:MyInputDir", "-outputdir:MyOutputDir", "-projectname:MyProjectName" };
            IArgumentsParser argumentsParser;
            ArgumentsParser.TryParse(arguments.ToArray(), out argumentsParser);

            var htmlDocGenerator = new HtmlDocGenerator(argumentsParser);
            Assert.AreEqual("MyInputDir", htmlDocGenerator.InputDir);
            Assert.AreEqual("MyOutputDir", htmlDocGenerator.OutputDir);
            Assert.AreEqual("MyProjectName", htmlDocGenerator.ProjectName);
        }
    }
}
