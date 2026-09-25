namespace GildedRose.Console.Updaters;

/// <summary>
/// Legendary items sit outside the ageing system entirely: they never have to be sold
/// and their quality never alters.
/// </summary>
/// <remarks>
/// Deliberately not derived from <see cref="ItemUpdater"/>. A legendary item is not an
/// item that ages by zero - it does not age at all, and its quality of 80 is exempt from
/// the ceiling that applies to ordinary goods.
/// </remarks>
public sealed class LegendaryItemUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        // Intentionally empty.
    }
}
