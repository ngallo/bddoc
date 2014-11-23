using System;
using System.IO;
using BDDoc.Core;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.Generator.UnitTest.Core
{
    [ExcludeFromCodeCoverage]
    public class ConsoleLoggerTests
    {
        //Tests

        [Test]
        public void Logging_InvalidMessage_AnExceptionIsThrown()
        {
            var logger = new ConsoleLogger();
            Assert.Throws<ArgumentNullException>(() => logger.Info(null));
            Assert.Throws<ArgumentNullException>(() => logger.Info(string.Empty));
            Assert.Throws<ArgumentNullException>(() => logger.Error(null));
            Assert.Throws<ArgumentNullException>(() => logger.Error(string.Empty));
        }

        [Test]
        public void Logging_ValidMessage_MessageLogged()
        {
            var logger = new ConsoleLogger();
            
            var newOut = new StringWriter();
            Console.SetOut(newOut);

            const string str1 = "InfoMessage";
            logger.Info(str1);
            Assert.AreEqual(str1 +"\r\n", newOut.ToString());

            newOut = new StringWriter();
            Console.SetOut(newOut);

            const string str2 = "ErrorMessage";
            logger.Info(str2);
            Assert.AreEqual(str2 + "\r\n", newOut.ToString());

            
        }
    }
}
