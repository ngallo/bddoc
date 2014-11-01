
namespace BDDoc
{
    public static class ObjectExtensions
    {
        //Methods

        public static PlainScenario CreatePlainScenario(this object instance)
        {
            return new PlainScenario();
        }

        public static FluentScenario CreateFluentScenario(this object instance)
        {
            return new FluentScenario();
        }
    }
}
