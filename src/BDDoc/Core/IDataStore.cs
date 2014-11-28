using BDDoc.Core.Documents;

namespace BDDoc.Core
{
    internal interface IDataStore
    {
        //Methods

        void Save(StoryDocument storyDocument, ScenarioDocument scenarioDocument);
    }
}
