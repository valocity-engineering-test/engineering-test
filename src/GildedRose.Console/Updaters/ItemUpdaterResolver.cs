namespace GildedRose.Console.Updaters;

/// <summary>
/// Selects the ageing rules that apply to an item, by name.
/// </summary>
/// <remarks>
/// Name matching is the only signal available: the item type carries no category and
/// cannot be extended. Matching is ordinal and case sensitive, mirroring the comparisons
/// the original implementation made.
/// </remarks>
public sealed class ItemUpdaterResolver
{
    public const string AgedBrie = "Aged Brie";
    public const string Sulfuras = "Sulfuras, Hand of Ragnaros";
    public const string BackstagePass = "Backstage passes to a TAFKAL80ETC concert";

    private static readonly IItemUpdater Default = new DefaultItemUpdater();

    private static readonly IReadOnlyDictionary<string, IItemUpdater> ByName =
        new Dictionary<string, IItemUpdater>(StringComparer.Ordinal)
        {
            [AgedBrie] = new AgedBrieUpdater(),
            [Sulfuras] = new LegendaryItemUpdater(),
            [BackstagePass] = new BackstagePassUpdater()
        };

    public IItemUpdater Resolve(Item item)
        => ByName.TryGetValue(item.Name, out var updater) ? updater : Default;
}
