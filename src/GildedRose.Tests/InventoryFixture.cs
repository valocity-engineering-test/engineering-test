using GildedRose.Console;

namespace GildedRose.Tests;

/// <summary>
/// Drives <see cref="Program.UpdateQuality"/> for a single item so the tests can state
/// "given this item, after N days, expect this" without repeating the setup.
/// </summary>
public static class InventoryFixture
{
    public static Item AfterOneDay(string name, int sellIn, int quality)
        => AfterDays(1, name, sellIn, quality);

    public static Item AfterDays(int days, string name, int sellIn, int quality)
    {
        var app = new Program
        {
            Items = new List<Item> { new Item { Name = name, SellIn = sellIn, Quality = quality } }
        };

        for (var day = 0; day < days; day++)
        {
            app.UpdateQuality();
        }

        return app.Items[0];
    }
}
