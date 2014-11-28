using System;

namespace BDDoc.Core
{
    public abstract class BDDocAttribute : Attribute, IBDDocAttrib
    {
        //Constructors

        private const string CAttribute = "ATTRIBUTE";

        //Constructors

        protected BDDocAttribute(string text, int order)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            Text = text;
            Order = order;
        }

        //Properties

        public string Key
        {
            get
            {
                var key = GetType().Name;
                if (key.ToUpper().EndsWith(CAttribute))
                {
                    key = key.Substring(0, key.Length - 9);
                }
                return key;
            }
        }

        public string Text { get; private set; }

        public int Order { get; set; }
    }
}
