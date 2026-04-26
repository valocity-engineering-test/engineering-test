using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Conjured items degrade in quality twice as fast as normal items.
    /// </summary>
    public class ConjuredItem : IItemBehavior
    {
        public void Update(Item item)
        {
            DecreaseQuality(item, 2);
            item.SellIn--;

            if (item.SellIn < 0)
                DecreaseQuality(item, 2);
        }

        private void DecreaseQuality(Item item, int amount)
        {
            item.Quality = Math.Max(0, item.Quality - amount);
        }
    }

}
