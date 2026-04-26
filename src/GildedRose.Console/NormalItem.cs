using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Normal items decrease in quality by 1 each day.
    /// After SellIn passes, they decrease twice as fast.
    /// </summary>
    public class NormalItem : IItemBehavior
    {
        public void Update(Item item)
        {
            DecreaseQuality(item, 1);
            item.SellIn--;

            if (item.SellIn < 0)
                DecreaseQuality(item, 1);
        }

        private void DecreaseQuality(Item item, int amount)
        {
            item.Quality = Math.Max(0, item.Quality - amount);
        }
    }

}
