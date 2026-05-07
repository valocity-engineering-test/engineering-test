namespace GildedRose.Console.QualityUpdaters;

public class ConjuredItemQualityUpdater : ItemQualityUpdaterBase
{
    public override void Update(Item item)
    {
        DecreaseSellIn(item);

        var qualityDecreaseAmount = IsExpired(item) ? 4 : 2;

        DecreaseQuality(item, qualityDecreaseAmount);
    }
}