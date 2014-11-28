using BDDoc.Core.Documents;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BDDoc.UnitTest.Core.Documents
{
    [ExcludeFromCodeCoverage]
    public class StoryDocumentTests
    {
        //Tests

        [Test]
        public void CreateStoryDocument_WithInvalidParameters_AnExecptionIsThrown()
        {
            const string text = "TXT";

            Assert.Throws<ArgumentNullException>(() => new StoryDocument(null, text));
            Assert.Throws<ArgumentNullException>(() => new StoryDocument(string.Empty, text));
            Assert.Throws<ArgumentNullException>(() => new StoryDocument(text, null));
            Assert.Throws<ArgumentNullException>(() => new StoryDocument(text, string.Empty));
            Assert.Throws<ArgumentNullException>(() => new StoryDocument(null, null));
            Assert.Throws<ArgumentNullException>(() => new StoryDocument(string.Empty, string.Empty));


            var storyDoc = new StoryDocument(text, text);
            Assert.Throws<ArgumentNullException>(() => storyDoc.AddItem(null, text));
            Assert.Throws<ArgumentNullException>(() => storyDoc.AddItem(string.Empty, text));
            Assert.Throws<ArgumentNullException>(() => storyDoc.AddItem(text, null));
            Assert.Throws<ArgumentNullException>(() => storyDoc.AddItem(text, string.Empty));
        }

        [Test]
        public void CreateStoryDocument_WithValidParameters_InstanceIsInitialized()
        {
            const string fileName = "FILENAME";
            const string text = "TXT";
            const string key = "KEY";
            const string value = "Value";

            var storyDoc = new StoryDocument(fileName, text);

            Assert.AreEqual(fileName, storyDoc.FileName);
            Assert.AreEqual(text, storyDoc.Text);

            for (var i = 1; i < 20; i++)
            {
                var key1 = string.Format("{0},{1}", key, i);
                var value1 = string.Format("{0},{1}", value, i);

                storyDoc.AddItem(key1, value1);

                Assert.IsTrue(storyDoc.Count() == i);

                Assert.AreEqual(key1, storyDoc.ElementAt(i - 1).Item1);
                Assert.AreEqual(value1, storyDoc.ElementAt(i - 1).Item2);
            }

            storyDoc = new StoryDocument(fileName, text);

            const int index = 1;
            foreach (var item in storyDoc)
            {
                var tuple = (Tuple<string, string>)item;
                var key1 = string.Format("{0},{1}", key, index);
                var value1 = string.Format("{0},{1}", value, index);

                storyDoc.AddItem(key1, value1);

                Assert.IsTrue(storyDoc.Count() == index);

                Assert.AreEqual(key1, tuple.Item1);
                Assert.AreEqual(value1, tuple.Item2);
            }
        }
    }
}
