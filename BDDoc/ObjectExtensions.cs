using BDDoc.Core;

namespace BDDoc
{
    public static class ObjectExtensions
    {
        //Methods

        public static PlainScenario CreateScenario(this object instance)
        {
            var scenarioFactory = ScenarioFactory.CreateInstance();
            return scenarioFactory.CreateScenario();
        }
    }
}
