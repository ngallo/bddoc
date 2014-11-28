using System;

namespace BDDoc.Core
{
    internal sealed class ScenarioStep
    {
        //Constructors

        public ScenarioStep(ScenarioStepType stepType, int order, string text)
        {
            if (order < 0)
            {
                throw new ArgumentException();
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            StepType = stepType;
            Order = order;
            Text = text;
        }

        //Properties

        public ScenarioStepType StepType { get; private set; }

        public int Order { get; private set; }
        
        public string Text { get; private set; }
    }
}
