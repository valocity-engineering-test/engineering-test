namespace GildedRose.Console.Updaters;

/// <summary>
/// Ordinary goods: they lose quality as they age, and twice as fast once the sell-by
/// date has passed.
/// </summary>
public class DefaultItemUpdater : ItemUpdater
{
    /// <summary>
    /// Quality lost per day before the sell-by date. Categories that decay faster than
    /// ordinary goods express that by raising this rate.
    /// </summary>
    protected virtual int DegradationRate => 1;

    protected override int CalculateQuality(Item item, bool expired)
        => item.Quality - DegradationRate * (expired ? 2 : 1);
}
