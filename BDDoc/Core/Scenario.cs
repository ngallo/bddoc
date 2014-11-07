using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace BDDoc.Core
{
    public abstract class Scenario
    {
        //Fields

        private int _isFrozen;
        private int _stepsCounter;
        private readonly IList<ScenarioStep> _steps;

        //Constructors

        protected Scenario(StoryInfoAttribute storyInfoAttribute, IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes)
        {
            if ((storyInfoAttribute == null) || (storyAttributes == null) || (storyAttributes.Count == 0) 
                || (scenarioAttributes == null) || (scenarioAttributes.Count == 0))
            {
                throw new ArgumentNullException();
            }
            _steps = new List<ScenarioStep>();
            StoryInfoAttribute = storyInfoAttribute;
            StoryAttributes = new ReadOnlyCollection<IStoryAttrib>(storyAttributes);
            ScenarioAttributes = new ReadOnlyCollection<IScenarioAttrib>(scenarioAttributes);
        }

        //Properties

        public StoryInfoAttribute StoryInfoAttribute { get; private set; }

        public ReadOnlyCollection<IStoryAttrib> StoryAttributes { get; private set; }

        public ReadOnlyCollection<IScenarioAttrib> ScenarioAttributes { get; private set; }

        //Methods

        private void CanUpdateScenario()
        {
            if (Interlocked.CompareExchange(ref _isFrozen, 1, 1) == 1)
            {
                throw new InvalidOperationException();
            }
        }

        private void AddStep(ScenarioStepType stepType, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            CanUpdateScenario();
            var stepIndex = Interlocked.Increment(ref _stepsCounter);
            var scenarioStep = new ScenarioStep(stepType, stepIndex, text);
            _steps.Add(scenarioStep);
        }

        internal ScenarioStep[] GetAllSteps()
        {
            return _steps.ToArray();
        }

        protected void Freeze()
        {
            if (Interlocked.Exchange(ref _isFrozen, 1) == 1)
            {
                throw new InvalidOperationException();
            }
        }

        protected void Save()
        {
        }

        protected void AddGivenStep(string text)
        {
            AddStep(ScenarioStepType.Given, text);
        }

        protected void AddAndStep(string text)
        {
            AddStep(ScenarioStepType.And, text);
        }

        protected void AddWhenStep(string text)
        {
            AddStep(ScenarioStepType.When, text);
        }

        protected void AddThenStep(string text)
        {
            AddStep(ScenarioStepType.Then, text);
        }
    }
}
