using GildedRose.Console.Helpers;

namespace GildedRose.Console.Updaters;

public class BackstagePassUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        QualityHelper.Increase(item, 1);

        if (item.SellIn < 11)
        {
            QualityHelper.Increase(item, 1);
        }

        if (item.SellIn < 6)
        {
            QualityHelper.Increase(item, 1);
        }

        item.SellIn--;

        if (item.SellIn < 0)
        {
            item.Quality = 0;
        }
    }
}
