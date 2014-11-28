using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BDDoc.Core
{
    internal class IoC
    {
        //Fields

        private static readonly Dictionary<Type, Type> MappedTypes = new Dictionary<Type, Type>();

        //Methods

        private static object CreateInstance(Type type, object[] parameters)
        {
            if ((type == null) || (parameters == null))
            {
                throw new ArgumentNullException();
            }
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var constructors = type.GetConstructors(bindingFlags);
            foreach (var constructorInfo in constructors)
            {
                var parameterInfos = constructorInfo.GetParameters();
                if ((!parameters.Any()) || (parameterInfos.Count() != parameters.Count())) continue;

                var constructorFound = true;
                for (var i = 0; i < parameters.Count(); i++)
                {
                    if (parameterInfos.ElementAt(i).ParameterType.IsInstanceOfType(parameters.ElementAt(i))) continue;
                    constructorFound = false;
                    break;
                }
                if (!constructorFound) continue;

                var instance = constructorInfo.Invoke(parameters);
                return instance;
            }
            return null;
        }

        public static void Map<T1, T2>()
            where T2 : T1
        {
            if (MappedTypes.ContainsKey(typeof(T1)))
            {
                throw new InvalidOperationException();
            }
            MappedTypes.Add(typeof(T1), typeof(T2));
        }

        public static T Resolve<T>()
        {
            return Resolve<T>(null);
        }

        public static T Resolve<T>(object[] parameters)
        {
            var type = typeof(T);
            if (MappedTypes.ContainsKey(type))
            {
                type = MappedTypes[type];
            }
            if ((parameters != null) && (parameters.Any()))
            {
                return (T)CreateInstance(type, parameters);
            }
            var instance = (T)Activator.CreateInstance(type);
            return instance;
        }
    }
}
