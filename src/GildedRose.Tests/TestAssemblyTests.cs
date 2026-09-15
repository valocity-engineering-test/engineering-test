using GildedRose.Console;
using Xunit;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    //Use generative AI to generate below test data.
    [Theory]
    [InlineData("Elixir of the Mongoose", 5, 7, 4, 6)]
    [InlineData("Elixir of the Mongoose", 0, 7, -1, 5)]
    [InlineData("Elixir of the Mongoose", 0, 1, -1, 0)]
    [InlineData("Aged Brie", 2, 0, 1, 1)]
    [InlineData("Aged Brie", 0, 49, -1, 50)]
    [InlineData("Aged Brie", 2, 50, 1, 50)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 11, 20, 10, 21)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 10, 20, 9, 22)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 5, 20, 4, 23)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 0, 20, -1, 0)]
    [InlineData("Backstage passes to a TAFKAL80ETC concert", 5, 49, 4, 50)]
    [InlineData("Conjured Mana Cake", 3, 6, 2, 4)]
    [InlineData("Conjured Mana Cake", 0, 6, -1, 2)]
    [InlineData("Conjured Mana Cake", 0, 3, -1, 0)]
    public void UpdateQuality_applies_item_rules(
        string name,
        int sellIn,
        int quality,
        int expectedSellIn,
        int expectedQuality)
    {
        var item = new Item { Name = name, SellIn = sellIn, Quality = quality };
        var app = new Program { Items = new List<Item> { item } };

        app.UpdateQuality();

        Assert.Equal(expectedSellIn, item.SellIn);
        Assert.Equal(expectedQuality, item.Quality);
    }

    [Fact]
    public void UpdateQuality_does_not_change_sulfuras()
    {
        var item = new Item
        {
            Name = "Sulfuras, Hand of Ragnaros",
            SellIn = 0,
            Quality = 80
        };
        var app = new Program { Items = new List<Item> { item } };

        app.UpdateQuality();

        Assert.Equal(0, item.SellIn);
        Assert.Equal(80, item.Quality);
    }
}
