
using System;

namespace BDDoc.Samples.Warehouse
{
    public abstract class Item
    {
        //Constructors

        protected Item(Color color)
        {
            Id = Guid.NewGuid().ToString();
            Color = color;
        }

        //Properties

        public string Id { get; private set; }

        public Color Color { get; private set; }
    }
}
