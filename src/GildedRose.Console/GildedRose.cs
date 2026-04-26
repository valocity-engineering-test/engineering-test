using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GildedRoseApp.Console
{
    public class GildedRose
    {
        private readonly IList<Item> _items;

        // Public property so tests (and callers) can access the items
        public IList<Item> Items => _items;

        public GildedRose(IList<Item> items)
        {
            _items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in _items)
            {
                IItemBehavior behavior = GetBehavior(item);
                behavior.Update(item);
            }
        }

        private IItemBehavior GetBehavior(Item item)
        {
            if (item.Name == "Aged Brie") return new AgedBrie();
            if (item.Name == "Backstage passes to a TAFKAL80ETC concert") return new BackstagePass();
            if (item.Name == "Sulfuras, Hand of Ragnaros") return new Sulfuras();
            if (item.Name.StartsWith("Conjured")) return new ConjuredItem();
            return new NormalItem();
        }
    }
}
