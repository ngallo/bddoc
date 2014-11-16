using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    [StoryInfo(StoryId)]
    public class InvalidObjectExtensionsTests : IStory
    {
        //Constants

        private const string StoryId = "StoryID";
        private const string ScenarioText = "This scenario's text is used just for testing purpose";
        private const int ScenarioOrder = 22;

        //Tests

        [Test]
        [Scenario(ScenarioText, Order = ScenarioOrder)]
        public void CreatePlainScenario_UsingTheObjectExtensionsWithoutDecorateWithAScenarioAttributeAndStoryAttribute_AnExceptionIsThrown()
        {
            var ex = Assert.Throws<BDDocException>(() => this.CreateScenario());
            Assert.AreEqual(Constants.CExceptionMessageMissingStoryAttribute, ex.Message);
        }

        [Test]
        public void CreatePlainScenario_UsingTheObjectExtensionsWithoutDecorateWithAScenarioAttribute_AnExceptionIsThrown()
        {
            var ex = Assert.Throws<BDDocException>(() => this.CreateScenario());
            Assert.AreEqual(Constants.CExceptionMessageMissingScenarioAttribute, ex.Message);
        }
    }
}
