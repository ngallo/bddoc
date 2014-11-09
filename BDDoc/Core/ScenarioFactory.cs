using BDDoc.Core.Reflection;
using System;
using System.Collections.Generic;

namespace BDDoc.Core
{
    internal sealed class ScenarioFactory
    {
        //Fields

        private readonly IReflectionHelper _reflectionHelper;
        private readonly IDataStore _dataStore;

        //Constructors

        internal ScenarioFactory(IReflectionHelper reflectionHelper, IDataStore dataStore)
        {
            if ((reflectionHelper == null) || (dataStore == null))
            {
                throw new ArgumentNullException();
            }
            _reflectionHelper = reflectionHelper;
            _dataStore = dataStore;
        }

        //Methods

        public static ScenarioFactory CreateInstance()
        {
            var reflectionHelper = new ReflectionHelper();
            var dataStore = new DataStore();
            return new ScenarioFactory(reflectionHelper, dataStore);
        }

        public PlainScenario CreateScenario()
        {
            StoryInfoAttribute storyInfoAttribute;
            IList<IStoryAttrib> storyAttribs;
            IList<IScenarioAttrib> scenarioAttribs;
            _reflectionHelper.RetrieveStoryAttributes(out storyInfoAttribute, out storyAttribs, out scenarioAttribs);
            var plainScenario = new PlainScenario(storyInfoAttribute, storyAttribs, scenarioAttribs);
            plainScenario.AttachDataStore(_dataStore);
            return plainScenario;
        }
    }
}
