using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Backstage passes increase in quality as the concert approaches:
    /// +1 normally, +2 when SellIn <= 10, +3 when SellIn <= 5.
    /// Quality drops to 0 after the concert.
    /// </summary>
    public class BackstagePass : IItemBehavior
    {
        public void Update(Item item)
        {
            IncreaseQuality(item, 1);

            if (item.SellIn <= 10)
                IncreaseQuality(item, 1);

            if (item.SellIn <= 5)
                IncreaseQuality(item, 1);

            item.SellIn--;

            if (item.SellIn < 0)
                item.Quality = 0;
        }

        private void IncreaseQuality(Item item, int amount)
        {
            item.Quality = Math.Min(50, item.Quality + amount);
        }
    }

}
