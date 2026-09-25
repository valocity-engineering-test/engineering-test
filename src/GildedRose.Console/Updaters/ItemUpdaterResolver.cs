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

    /// <summary>
    /// Conjured goods are a category rather than a single product, so they are matched on
    /// the prefix. The trailing space keeps "Conjured Mana Cake" in and leaves names that
    /// merely share the stem, such as "Conjuration Robes", out.
    /// </summary>
    public const string ConjuredPrefix = "Conjured ";

    private static readonly IItemUpdater Default = new DefaultItemUpdater();
    private static readonly IItemUpdater Conjured = new ConjuredItemUpdater();

    private static readonly IReadOnlyDictionary<string, IItemUpdater> ByName =
        new Dictionary<string, IItemUpdater>(StringComparer.Ordinal)
        {
            [AgedBrie] = new AgedBrieUpdater(),
            [Sulfuras] = new LegendaryItemUpdater(),
            [BackstagePass] = new BackstagePassUpdater()
        };

    public IItemUpdater Resolve(Item item)
    {
        if (ByName.TryGetValue(item.Name, out var updater))
        {
            return updater;
        }

        return item.Name.StartsWith(ConjuredPrefix, StringComparison.Ordinal)
            ? Conjured
            : Default;
    }
}
