
namespace BDDoc
{
    internal class Constants
    {
        //Constants

        public const string CExceptionMessageInvalidStep = "Invalid ScenarioStepType.{0} scenario step. Order of the steps has to be in the following order Given, And, When, And, Then, And.";
        public const string CExceptionMessageMissingStoryAttribute = "Missing StoryAttribute. Please decorate your test class with StoryAttribute.";
        public const string CExceptionMessageMissingScenarioAttribute = "Missing ScenarioAttribute. Please decorate your test method with ScenarioAttribute.";
        public const string CExceptionMessageFrozenScenario = "Scenario is frozen can not be updated";
        public const string CExceptionMessageInvalidState = "Scenario has an invalid state.";
        public const string CExceptionMessageCanNotSaveNotCompleted = "Scenario can not be saved as it is not marked as completed.";
    }
}
