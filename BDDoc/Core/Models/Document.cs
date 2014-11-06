using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BDDoc.Core.Models
{
    internal abstract class Document : IEnumerable<Tuple<string,string>>
    {
        //Fields

        private IList<Tuple<string,string>> _items;

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

        private IList<Tuple<string, string>> Items
        {
            get { return _items ?? (_items = new List<Tuple<string, string>>()); }
        }

        public string Text { get; private set; }

        //Methods

        IEnumerator<Tuple<string, string>> IEnumerable<Tuple<string, string>>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public bool AddItem(string key, string text)
        {
            if ((string.IsNullOrWhiteSpace(key)) || (string.IsNullOrWhiteSpace(text)))
            {
                throw new ArgumentNullException();
            }
            if (Items.Select((i) => i.Item1.Equals(key)).Any())
            {
                return false;
            }
            var keyValue = new Tuple<string, string>(key, text);
            Items.Add(keyValue);
            return true;
        }
    }
}
