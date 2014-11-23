using BDDoc.Core.Arguments;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Generator.UnitTest.Core.Arguments
{
    [ExcludeFromCodeCoverage]
    public class ArgumentParserTests
    {
        //Tests

        [Test]
        public void InitializingArgumentParser_WithInvalidParameters_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new ArgumentsParser(string.Empty));
            IEnumerable<Tuple<string, object>> arguments = null;
            Assert.Throws<ArgumentNullException>(() => new ArgumentsParser(arguments));
        }

        [Test]
        public void InitializingArgumentParser_WithValidParameters_ArgumentParserIsInitialized()
        {
            Program.Initialize();

            var arguments = new List<string> { "-inputdir:MyInputDir", "-outputdir:MyOutputDir", "-projectname:MyProjectName" };
            IArgumentsParser argumentsParser;
            ArgumentsParser.TryParse(arguments.ToArray(), out argumentsParser);
            Assert.AreEqual("MyInputDir", argumentsParser[ArgumentsParser.CInputDir]);
            Assert.AreEqual("MyOutputDir", argumentsParser[ArgumentsParser.COutputDir]);
            Assert.AreEqual("MyProjectName", argumentsParser[ArgumentsParser.CProjectName]);
        }
    }
}
