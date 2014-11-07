using System.Diagnostics.CodeAnalysis;
using BDDoc.Reflection;
using NUnit.Framework;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    [StoryInfo(StoryId)]
    public class InvalidObjectExtensionsTests
    {
        //Constants

        private const string StoryId = "StoryID";
        private const string ScenarioText = "This scenario's text is used just for testing purpose";
        private const int ScenarioOrder = 22;

        //Tests

        [Scenario(ScenarioText, Order = ScenarioOrder)]
        [Test]
        public void CreatePlainScenario_UsingTheObjectExtensionsWithoutDecorateWithAScenarioAttribute_AnExceptionIsThrown()
        {
            Assert.Throws<BDDocConfigurationException>(() => this.CreateScenario(), ReflectionHelper.CMissingStoryAttributeAttributeExceptionMessage);
        }

        [Scenario(ScenarioText, Order = ScenarioOrder)]
        [Test]
        public void CreatePlainScenario_UsingTheObjectExtensionsWithoutDecorateWithAScenarioAttributeAndStoryAttribute_AnExceptionIsThrown()
        {
            Assert.Throws<BDDocConfigurationException>(() => this.CreateScenario(), ReflectionHelper.CMissingStoryAttributeAttributeExceptionMessage);
        }
    }
}
