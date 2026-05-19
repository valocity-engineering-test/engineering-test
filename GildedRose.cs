using System.Collections.Generic;

namespace GildedRose
{
    public class GildedRose
    {
        public IList<Item> Items;

        public GildedRose(IList<Item> items)
        {
            Items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                GetUpdater(item).Update(item);
            }
        }

        private static IItemUpdater GetUpdater(Item item)
        {
            return item.Name switch
            {
                "Aged Brie" => new AgedBrieUpdater(),

                "Backstage passes to a TAFKAL80ETC concert"
                    => new BackstagePassUpdater(),

                "Sulfuras, Hand of Ragnaros"
                    => new SulfurasUpdater(),

                _ when item.Name.StartsWith("Conjured")
                    => new ConjuredItemUpdater(),

                _ => new NormalItemUpdater()
            };
        }
    }
}