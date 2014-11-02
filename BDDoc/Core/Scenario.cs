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

        private int _isFrozen = 0;
        private int _stepsCounter = 0;
        private readonly IList<ScenarioStep> _steps;

        //Constructors

        protected Scenario(IList<IStoryAttrib> storyAttributes, IList<IScenarioAttrib> scenarioAttributes)
        {
            if (storyAttributes == null || (storyAttributes.Count == 0) 
                || (scenarioAttributes == null) || (scenarioAttributes.Count == 0))
            {
                throw new ArgumentNullException();
            }
            StoryAttributes = new ReadOnlyCollection<IStoryAttrib>(storyAttributes);
            ScenarioAttributes = new ReadOnlyCollection<IScenarioAttrib>(scenarioAttributes);
            _steps = new List<ScenarioStep>();
        }

        //Properties

        public ReadOnlyCollection<IStoryAttrib> StoryAttributes { get; private set; }

        public ReadOnlyCollection<IScenarioAttrib> ScenarioAttributes { get; private set; }

        //Methods

        private void CanUpdateScenario()
        {
            if (_isFrozen == 1)
            {
                throw new InvalidOperationException();
            }
        }

        private void AddStep(ScenarioStepType stepType, string text)
        {
            CanUpdateScenario();
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
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
