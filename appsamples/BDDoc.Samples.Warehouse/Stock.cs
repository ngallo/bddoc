using System;
using System.Collections.Generic;

namespace BDDoc.Samples.Warehouse
{
    public class Stock
    {
        //Fields

        private readonly Lazy<IList<Item>> _items = new Lazy<IList<Item>>();
       
        //Methods

        public bool AddItem(Item item)
        {
            if (item == null) throw new ArgumentNullException();
            
            if (_items.Value.Contains(item))
            {
                return false;
            }
            _items.Value.Add(item);
            return true;
        }
    }
}
