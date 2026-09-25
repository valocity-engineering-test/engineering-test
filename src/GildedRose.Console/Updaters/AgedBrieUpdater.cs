namespace GildedRose.Console.Updaters;

/// <summary>
/// Aged Brie improves with age, and improves faster once its sell-by date has passed.
/// </summary>
public sealed class AgedBrieUpdater : ItemUpdater
{
    protected override int CalculateQuality(Item item, bool expired)
        => item.Quality + (expired ? 2 : 1);
}
