using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BDDoc.Core.Arguments
{
    internal class ArgumentsParser : IArgumentsParser, IEnumerable
    {
        //Constants

        public const string CArgPattern = "-{0}:";
        public const string CInputDir = "inputdir";
        public const string COutputDir = "outputdir";

        //Fields

        private static readonly string[] Arguments = { CInputDir, COutputDir };

        private readonly Dictionary<string, object> _arguments;

        //Constructors

        internal ArgumentsParser(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        
        internal ArgumentsParser(params Tuple<string, object>[] arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException();
            }
            _arguments = new Dictionary<string, object>();
            foreach (var argument in arguments)
            {
                _arguments.Add(argument.Item1, argument.Item2);
            }
        }

        //Indexers

        public object this[string key]
        {
            get
            {
                return _arguments.ContainsKey(key) ? _arguments[key] : null;
            }
        }

        //Properties

        public string ErrorMessage { get; private set; }

        //Methods

        public static string GetArgumentName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException();
            }
            return string.Format(CArgPattern, name);
        }

        public static bool Validate(string name, string value, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException();
            }
            if ((name.Equals(CInputDir)) || (name.Equals(COutputDir)))
            {
                try
                {
                    if (!Directory.Exists(value))
                    {
                        Directory.CreateDirectory(value);
                        var logger = IoC.Resolve<ILogger>();
                        var message = string.Format("Created directory {0}", value);
                        logger.Info(message);
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return false;
                }
            }
            errorMessage = null;
            return true;
        }

        public static bool TryParse(string[] args, out ArgumentsParser argumentsParser)
        {
            var arguments = new List<Tuple<string, object>>();

            foreach (var argument in Arguments)
            {
                string errorMessage;
                var argName = GetArgumentName(argument);
                var inputArg = args.FirstOrDefault(s => s.ToUpper().StartsWith(argName.ToUpper()));
                if (inputArg  == null)
                {
                    errorMessage = string.Format("Missing argument {0}.", argName);
                    argumentsParser = new ArgumentsParser(errorMessage);
                    return false;
                }
                var inputArgValue = inputArg.Remove(0, argName.Length);
                if (string.IsNullOrWhiteSpace(inputArgValue))
                {
                    errorMessage = string.Format("Invalid argument ({0}).", argName);
                    argumentsParser = new ArgumentsParser(errorMessage);
                    return false;
                }
                if (!Validate(argument, inputArgValue, out errorMessage))
                {
                    argumentsParser = new ArgumentsParser(errorMessage);
                    return false;
                }


                arguments.Add(new Tuple<string, object>(argument, inputArgValue));
            }

            argumentsParser = new ArgumentsParser(arguments.ToArray());
            
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return _arguments.GetEnumerator();
        }
    }
}
