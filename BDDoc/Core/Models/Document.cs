using System;
using System.Collections.Generic;

namespace BDDoc.Core.Models
{
    internal abstract class Document
    {
        //Fields

        private Dictionary<string, string> _items;

        //Constructors

        protected Document(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception();
            }
            Text = text;
        }

        //Properties

        private Dictionary<string, string> Items
        {
            get { return _items ?? (_items = new Dictionary<string, string>()); }
        }

        public string Text { get; private set; }

        //Methods

        public bool AddItem(string key, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException();
            }
            if (!Items.ContainsKey(key))
            {
                return false;
            }
            _items.Add(key, text);
            return true;
        }
    }
}

