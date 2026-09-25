namespace GildedRose.Console.Updaters;

/// <summary>
/// Backstage passes gain value as the concert approaches, then become worthless the
/// moment it has taken place.
/// </summary>
public sealed class BackstagePassUpdater : ItemUpdater
{
    private const int FinalWeekThreshold = 5;
    private const int SecondWeekThreshold = 10;

    protected override int CalculateQuality(Item item, bool expired)
    {
        if (expired)
        {
            return MinQuality;
        }

        return item.Quality + item.SellIn switch
        {
            <= FinalWeekThreshold => 3,
            <= SecondWeekThreshold => 2,
            _ => 1
        };
    }
}
