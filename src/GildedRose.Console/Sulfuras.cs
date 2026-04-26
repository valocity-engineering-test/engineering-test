using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Sulfuras is a legendary item:
    /// Quality is always 80 and never changes.
    /// SellIn never decreases.
    /// </summary>
    public class Sulfuras : IItemBehavior
    {
        public void Update(Item item)
        {
            // Do nothing: Sulfuras never changes
        }
    }

}
