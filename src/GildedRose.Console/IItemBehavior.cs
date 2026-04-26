using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseApp.Console
{
    /// <summary>
    /// Defines the contract for item behavior.
    /// Each item type must implement how its quality and sell-in are updated.
    /// </summary>
    public interface IItemBehavior
    {
        void Update(Item item);
    }

}
