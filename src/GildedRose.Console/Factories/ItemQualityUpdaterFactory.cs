using GildedRose.Console.QualityUpdaters;

namespace GildedRose.Console.Factories;

public class ItemQualityUpdaterFactory
{
    private readonly IItemQualityUpdater _normalItemUpdater = new NormalItemQualityUpdater();
    private readonly IItemQualityUpdater _agedBrieUpdater = new AgedBrieQualityUpdater();
    private readonly IItemQualityUpdater _sulfurasUpdater = new SulfurasQualityUpdater();
    private readonly IItemQualityUpdater _backstagePassUpdater = new BackstagePassQualityUpdater();
    private readonly IItemQualityUpdater _conjuredItemUpdater = new ConjuredItemQualityUpdater();

    public IItemQualityUpdater GetUpdater(Item item)
    {
        if (item.Name == ItemNames.AgedBrie)
        {
            return _agedBrieUpdater;
        }

        if (item.Name == ItemNames.Sulfuras)
        {
            return _sulfurasUpdater;
        }

        if (item.Name == ItemNames.BackstagePass)
        {
            return _backstagePassUpdater;
        }

        if (item.Name.StartsWith(ItemNames.Conjured, StringComparison.OrdinalIgnoreCase))
        {
            return _conjuredItemUpdater;
        }

        return _normalItemUpdater;
    }
}