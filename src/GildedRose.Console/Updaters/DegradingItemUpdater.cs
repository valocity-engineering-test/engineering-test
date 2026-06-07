using GildedRose.Console.Helpers;

namespace GildedRose.Console.Updaters;

public abstract class DegradingItemUpdater : IItemUpdater
{
    protected abstract int DegradeRate { get; }

    public void Update(Item item)
    {
        QualityHelper.Decrease(item, DegradeRate);
        item.SellIn--;

        if (item.SellIn < 0)
        {
            QualityHelper.Decrease(item, DegradeRate);
        }
    }
}
