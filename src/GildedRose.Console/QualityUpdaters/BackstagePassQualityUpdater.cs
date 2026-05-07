namespace GildedRose.Console.QualityUpdaters;

public class BackstagePassQualityUpdater : ItemQualityUpdaterBase
{
    public override void Update(Item item)
    {
        if (item.SellIn <= 0)
        {
            item.Quality = 0;
            DecreaseSellIn(item);
            return;
        }

        var qualityIncreaseAmount = GetQualityIncreaseAmount(item.SellIn);

        IncreaseQuality(item, qualityIncreaseAmount);
        DecreaseSellIn(item);
    }

    private static int GetQualityIncreaseAmount(int sellIn)
    {
        if (sellIn <= 5)
        {
            return 3;
        }

        if (sellIn <= 10)
        {
            return 2;
        }

        return 1;
    }
}