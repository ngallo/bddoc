
namespace BDDoc.Core
{
    internal class IoC
    {
        //Methods

        public static T Resolve<T>()
            where T : class 
        {
            if (typeof(T) == typeof(ILogger))
            {
                return new ConsoleLogger() as T;
            }
            return default(T);
        }
    }
}
