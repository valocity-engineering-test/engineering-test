namespace GildedRose.Console.Updaters;

public static class ItemUpdaterFactory
{
    private const string AgedBrie = "Aged Brie";
    private const string BackstagePass = "Backstage passes to a TAFKAL80ETC concert";
    private const string Sulfuras = "Sulfuras, Hand of Ragnaros";

    public static IItemUpdater GetUpdater(Item item)
    {
        if (item.Name == Sulfuras)
        {
            return new SulfurasUpdater();
        }

        if (item.Name == AgedBrie)
        {
            return new AgedBrieUpdater();
        }

        if (item.Name == BackstagePass)
        {
            return new BackstagePassUpdater();
        }

        if (item.Name.StartsWith("Conjured"))
        {
            return new ConjuredItemUpdater();
        }

        return new NormalItemUpdater();
    }
}
