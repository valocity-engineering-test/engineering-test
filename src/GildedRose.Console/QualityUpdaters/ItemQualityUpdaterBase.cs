using System;

namespace GildedRose.Console.QualityUpdaters;

public abstract class ItemQualityUpdaterBase : IItemQualityUpdater
{
    private const int MinimumQuality = 0;
    private const int MaximumQuality = 50;

    public abstract void Update(Item item);

    protected static void DecreaseSellIn(Item item)
    {
        item.SellIn--;
    }

    protected static void IncreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Min(MaximumQuality, item.Quality + amount);
    }

    protected static void DecreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Max(MinimumQuality, item.Quality - amount);
    }

    protected static bool IsExpired(Item item)
    {
        return item.SellIn < 0;
    }
}