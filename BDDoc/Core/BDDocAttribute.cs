﻿using System;

namespace BDDoc.Core
{
    public abstract class BDDocAttribute : Attribute, IBDDocAttrib
    {
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

        public string Text { get; private set; }

        public int Order { get; set; }
    }
}
