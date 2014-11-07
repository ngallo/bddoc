using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class CustomExceptionTests
    {
        //Tests

        [Test]
        public void CreateCustomException_UsingValidParameters_InstanceIsInitialized()
        {
            Assert.DoesNotThrow(() => new BDDocConfigurationException());
            const string text = "TEXT";
            var exception = new BDDocConfigurationException(text);
            Assert.AreEqual(text, exception.Message);
        }
    }
}
