using BDDoc.Core;
using BDDoc.Core.Reflection;

namespace BDDoc
{
    public static class ObjectExtensions
    {
        //Constructors
        static ObjectExtensions()
        {
            IoC.Map<IReflectionHelper, ReflectionHelper>();
            IoC.Map<IDataStore, DataStore>();
        }

        //Methods

        public static PlainScenario CreateScenario(this IStory instance)
        {
            var scenarioFactory = ScenarioFactory.CreateInstance();
            return scenarioFactory.CreateScenario();
        }
    }
}
