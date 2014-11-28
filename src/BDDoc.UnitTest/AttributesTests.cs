using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BDDoc.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class AttributesTests
    {
        //Tests

        [Test]
        public void CreateAttribute_UsingNotValidParameters_AnExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new StoryInfoAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new StoryInfoAttribute(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new StoryAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new StoryAttribute(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new InOrderToAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new InOrderToAttribute(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new AsAAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new AsAAttribute(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new IWantToAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new IWantToAttribute(string.Empty));
            Assert.Throws<ArgumentNullException>(() => new ScenarioAttribute(null));
            Assert.Throws<ArgumentNullException>(() => new ScenarioAttribute(string.Empty));
        }

        [Test]
        public void CreateAttribute_UsingValidParameters_AttributeIsInitializedWithTheInputValue()
        {
            const string txt = "TEXT";
            const int order = 83;

            var storyInfoAttrib = new StoryInfoAttribute(txt);
            Assert.AreEqual(txt, storyInfoAttrib.StoryId);

            var storyAttrib = new StoryAttribute(txt) { Order = order };
            Assert.AreEqual(txt, storyAttrib.Text);
            Assert.AreEqual(order, storyAttrib.Order);

            var inOrderToAttrib = new InOrderToAttribute(txt) { Order = order };
            Assert.AreEqual(txt, inOrderToAttrib.Text);
            Assert.AreEqual(order, inOrderToAttrib.Order);

            var asAAttrib = new AsAAttribute(txt) { Order = order };
            Assert.AreEqual(txt, asAAttrib.Text);
            Assert.AreEqual(order, asAAttrib.Order);

            var iWantToAttrib = new IWantToAttribute(txt) { Order = order };
            Assert.AreEqual(txt, iWantToAttrib.Text);
            Assert.AreEqual(order, iWantToAttrib.Order);

            var scenarioAttrib = new ScenarioAttribute(txt) { Order = order };
            Assert.AreEqual(txt, scenarioAttrib.Text);
            Assert.AreEqual(order, scenarioAttrib.Order);
        }
    }
}
