namespace GildedRose.Console.Updaters;

/// <summary>
/// Base for items that participate in the normal ageing cycle: quality is recalculated,
/// held within the permitted range, and the sell-by date moves one day closer.
/// </summary>
public abstract class ItemUpdater : IItemUpdater
{
    protected const int MinQuality = 0;
    protected const int MaxQuality = 50;

    public void Update(Item item)
    {
        // Evaluated before the sell-in is decremented, which is what the subclasses'
        // thresholds are expressed against. An item is expired on the day its sell-in
        // reaches zero, not the day after.
        var expired = item.SellIn <= 0;

        item.Quality = Math.Clamp(CalculateQuality(item, expired), MinQuality, MaxQuality);
        item.SellIn--;
    }

    /// <summary>
    /// The item's new quality, before it is held within the permitted range.
    /// </summary>
    protected abstract int CalculateQuality(Item item, bool expired);
}
