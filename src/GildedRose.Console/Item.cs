using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Represents a product in the store.
    /// </summary>
    public class Item
    {
        public string Name { get; set; } = "";
        public int SellIn { get; set; }
        public int Quality { get; set; }
    }
}
