using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Aged Brie increases in quality as it ages.
    /// Quality never exceeds 50.
    /// </summary>
    public class AgedBrie : IItemBehavior
    {
        public void Update(Item item)
        {
            IncreaseQuality(item, 1);
            item.SellIn--;

            if (item.SellIn < 0)
                IncreaseQuality(item, 1);
        }

        private void IncreaseQuality(Item item, int amount)
        {
            item.Quality = Math.Min(50, item.Quality + amount);
        }
    }

}
