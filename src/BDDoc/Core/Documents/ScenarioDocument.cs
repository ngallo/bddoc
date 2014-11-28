using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BDDoc.Core.Documents
{
    internal class ScenarioDocument : Document
    {
        //Fields

        private IList<StepDocument> _items;

        //Constructors

        public ScenarioDocument(string text) : base(text) { }

        //Properties

        private IList<StepDocument> Items
        {
            get { return _items ?? (_items = new List<StepDocument>()); }
        }

        public ReadOnlyCollection<StepDocument> Steps
        {
            get { return new ReadOnlyCollection<StepDocument>(Items); }
        }

        //Methods

        public void AddStep(ScenarioStepType stepType, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            var document = new StepDocument(stepType, text);
            Items.Add(document);
        }
    }
}
