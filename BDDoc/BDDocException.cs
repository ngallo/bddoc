using System;

namespace BDDoc
{
    public class BDDocException : Exception
    {
        //Constructors

        public BDDocException() { }

        public BDDocException(string message) : 
            base(message) { }
    }
}
