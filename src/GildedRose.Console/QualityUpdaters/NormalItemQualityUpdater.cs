namespace GildedRose.Console.QualityUpdaters;

public class NormalItemQualityUpdater : ItemQualityUpdaterBase
{
    public override void Update(Item item)
    {
        DecreaseSellIn(item);

        var qualityDecreaseAmount = IsExpired(item) ? 2 : 1;

        DecreaseQuality(item, qualityDecreaseAmount);
    }
}