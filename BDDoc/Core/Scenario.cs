using BDDoc.Core.Models;
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

        private readonly IList<ScenarioStep> _steps;

        private readonly object _syncRoot = new object();
        private int _isFrozen;
        private bool _isInvalidState;
        private int _stepsCounter;
        private int _lstStepType = -1;
        private int _lstBaseStepType = -1;
        private IDataStore _dataStore;

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
                throw new BDDocException(Constants.CExceptionMessageFrozenScenario);
            }
        }

        private void AddStep(ScenarioStepType stepType, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            CanUpdateScenario();
            var lastStep = -1;
            var stepIndex = -1;
            lock (_syncRoot)
            {
                lastStep = _lstStepType;
                _lstStepType = (int)stepType;
                stepIndex = ++_stepsCounter;
            }
            if (lastStep != (int)ScenarioStepType.And)
            {
                _lstBaseStepType = lastStep;
            }
            if (((lastStep == -1) && (stepType != ScenarioStepType.Given))
                || ((_lstBaseStepType >= (int)stepType) && ((int)stepType != (int)ScenarioStepType.And))
                || ((_lstBaseStepType == (int)ScenarioStepType.Given) && (stepType == ScenarioStepType.Then)))
            {
                Freeze();
                SetInvalidState();
                var stepName = Enum.GetName(typeof (ScenarioStepType), stepType);
                var message = string.Format(Constants.CExceptionMessageInvalidStep, stepName);
                throw new BDDocException(message);
            }
            var scenarioStep = new ScenarioStep(stepType, stepIndex, text);
            _steps.Add(scenarioStep);
        }

        protected void SetInvalidState()
        {
            _isInvalidState = true;
        }

        internal void AttachDataStore(IDataStore dataStore)
        {
            if (dataStore == null)
            {
                throw new ArgumentNullException();
            }
            _dataStore = dataStore;
        }

        internal ScenarioStep[] GetAllSteps()
        {
            return _steps.ToArray();
        }

        protected void Freeze()
        {
            if (Interlocked.Exchange(ref _isFrozen, 1) == 1)
            {
                throw new BDDocException(Constants.CExceptionMessageFrozenScenario);
            }
        }

        protected void Save()
        {
            if (_isInvalidState)
            {
                throw new BDDocException(Constants.CExceptionMessageInvalidState);
            }
            if (Interlocked.CompareExchange(ref _isFrozen, 1, 1) == 0)
            {
                throw new BDDocException(Constants.CExceptionMessageCanNotSaveNotCompleted);
            }
            if (_dataStore == null)
            {
                throw new InvalidOperationException();
            }

            var storyAttrib = StoryAttributes.First(a => a is StoryAttribute);
            var storyDocument = new StoryDocument(StoryInfoAttribute.StoryId, storyAttrib.Text);
            var orderedStoryAttrib = StoryAttributes.Where((a) => a != storyAttrib).OrderBy((a) => a.Order);
            foreach (var attrib in orderedStoryAttrib)
            {
                storyDocument.AddItem(attrib.Key, attrib.Text);
            }

            var scenarioAttrib = ScenarioAttributes.First(a => a is ScenarioAttribute);
            var scenarioDocument = new ScenarioDocument(scenarioAttrib.Text);
            var orderedScenarioAttrib = ScenarioAttributes.Where((a) => a != scenarioAttrib).OrderBy((a) => a.Order);
            foreach (var attrib in orderedScenarioAttrib)
            {
                scenarioDocument.AddItem(attrib.Key, attrib.Text);
            }

            var orderdSteps = _steps.OrderBy((s) => s.Order);
            foreach (var scenarioStep in orderdSteps)
            {
                scenarioDocument.AddStep(scenarioStep.StepType, scenarioStep.Text);
            }

            _dataStore.Save(storyDocument, scenarioDocument);
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
