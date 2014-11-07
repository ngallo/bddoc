using System;

namespace BDDoc
{
    public class BDDocConfigurationException : Exception
    {
        //Constructors

        public BDDocConfigurationException() { }

        public BDDocConfigurationException(string message) : 
            base(message) { }
    }
}
