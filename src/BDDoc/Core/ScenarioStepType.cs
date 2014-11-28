using System;

namespace BDDoc.Core
{
    [Flags]
    internal enum ScenarioStepType
    {
        Given = 0,
        And = 1,
        When = 2,
        Then = 4
    }
}
