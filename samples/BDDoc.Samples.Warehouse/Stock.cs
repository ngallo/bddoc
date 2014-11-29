using System;
using System.Collections.Generic;
using System.Linq;

namespace BDDoc.Samples.Warehouse
{
    public class Stock<T>
        where T : Item
    {
        //Fields

        private readonly Lazy<List<T>> _items = new Lazy<List<T>>();

        //Constructors

        public Stock(IEnumerable<T> items)
        {
            if (items != null)
            {
                _items.Value.AddRange(items);
            }
        }
       
        //Methods

        private bool InsertItem(T item)
        {
            if (item == null) throw new ArgumentNullException();

            if (_items.Value.Any(i => i.Id == item.Id)) return false;

            _items.Value.Add(item);
            return true;
        }

        private T PickItem(Color color)
        {
            var item = _items.Value.FirstOrDefault(i => i.Color == color);
            if (item != null)
            {
                _items.Value.Remove(item);
            }
            return item;
        }

        public int GetNumberOfLeftItems(Color color)
        {
            return _items.Value.Count(i => i.Color == color);
        }

        public T BuyItem(Color color)
        {
            return PickItem(color);
        }

        public bool ReturnItem(T item)
        {
            return InsertItem(item);
        }

        public T ReplaceItem(T item, Color newColor)
        {
            if (item == null) throw new ArgumentNullException();
            
            if (item.Color == newColor) return item;

            var newItem = PickItem(newColor);
            if (newItem == null) throw new InvalidOperationException();

            InsertItem(item);
            
            return newItem;
        }
    }
}
