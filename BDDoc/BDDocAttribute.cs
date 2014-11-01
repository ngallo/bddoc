using System;

namespace BDDoc
{
    public abstract class BDDocAttribute : Attribute
    {
        //Constructors

        protected BDDocAttribute(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }
            Text = text;
        }

        //Properties

        public string Text { get; private set; }
    }
}
