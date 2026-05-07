namespace GildedRose.Console.QualityUpdaters;

public class AgedBrieQualityUpdater : ItemQualityUpdaterBase
{
    public override void Update(Item item)
    {
        DecreaseSellIn(item);

        var qualityIncreaseAmount = IsExpired(item) ? 2 : 1;

        IncreaseQuality(item, qualityIncreaseAmount);
    }
}   