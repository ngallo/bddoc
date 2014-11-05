using System;
using System.Collections.Generic;

namespace BDDoc.Core.Models
{
    internal class ScenarioDocument : Document
    {
        //Fields

        private IList<StepDocument> _steps;

        //Constructors

        public ScenarioDocument(string text) : base(text) { }

        //Properties

        private IList<StepDocument> Steps
        {
            get { return _steps ?? (_steps = new List<StepDocument>()); }
        }

        //Methods

        public void AddStep(ScenarioStepType stepType, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            var document = new StepDocument(stepType, text);
            Steps.Add(document);
        }
    }
}
