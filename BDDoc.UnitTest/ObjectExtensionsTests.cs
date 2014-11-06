using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    [StoryInfo(StoryId)]
    [Story(StoryText, Order = StoryOrder)]
    public class ObjectExtensionsTests
    {
        //Constants

        private const string StoryId = "StoryID";
        private const string StoryText = "This story's text is used just for testing purpose";
        private const int StoryOrder = 11;
        private const string ScenarioText = "This scenario's text is used just for testing purpose";
        private const int ScenarioOrder = 22;

        //Tests

        [Test]
        [Scenario(ScenarioText, Order = ScenarioOrder)]
        public void CreatePlainScenario_UsingTheObjectExtensions_ANewPlainScenarioHasToBeInstanced()
        {
            var scenario = this.CreateScenario();
            Assert.NotNull(scenario);
            Assert.AreEqual(StoryId, scenario.StoryInfoAttribute.StoryId);
            Assert.AreEqual(1, scenario.StoryAttributes.Count);
            Assert.AreEqual(StoryText, scenario.StoryAttributes.First().Text);
            Assert.AreEqual(StoryOrder, scenario.StoryAttributes.First().Order);
            Assert.AreEqual(1, scenario.ScenarioAttributes.Count);
            Assert.AreEqual(ScenarioText, scenario.ScenarioAttributes.First().Text);
            Assert.AreEqual(ScenarioOrder, scenario.ScenarioAttributes.First().Order);
        }
    }
}
