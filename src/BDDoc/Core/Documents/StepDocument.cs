using System;

namespace BDDoc.Core.Documents
{
    internal class StepDocument
    {
        //Constructors

        public StepDocument(ScenarioStepType stepType, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            StepType = Enum.GetName(typeof (ScenarioStepType), stepType);
            Text = text;
        }

        //Properties

        public string StepType { get; private set; }

        public string Text { get; private set; }
    }
}
