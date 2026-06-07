using GildedRose.Console.Helpers;

namespace GildedRose.Console.Updaters;

public class AgedBrieUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        QualityHelper.Increase(item, 1);
        item.SellIn--;

        if (item.SellIn < 0)
        {
            QualityHelper.Increase(item, 1);
        }
    }
}
