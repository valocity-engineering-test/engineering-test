using GildedRose.Console;
using GildedRose.Console.Updaters;
using Xunit;

namespace GildedRose.Tests;

public class GildedRoseTests
{
    private static Program CreateApp(params Item[] items)
    {
        return new Program { Items = items };
    }

    [Fact]
    public void UpdateQuality_UpdatesAllItemsInList()
    {
        var app = CreateApp(
            new Item { Name = "Normal Item", SellIn = 5, Quality = 10 },
            new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].Quality);
        Assert.Equal(4, app.Items[1].Quality);
    }

    [Fact]
    public void NormalItem_LosesOneQualityEachDay()
    {
        var app = CreateApp(new Item { Name = "Normal Item", SellIn = 5, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(9, app.Items[0].Quality);
    }

    [Fact]
    public void NormalItem_LosesExtraQualityAfterSellByDate()
    {
        var app = CreateApp(new Item { Name = "Normal Item", SellIn = 0, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(8, app.Items[0].Quality);
    }

    [Fact]
    public void NormalItem_QualityNeverGoesBelowZero()
    {
        var app = CreateApp(new Item { Name = "Normal Item", SellIn = 0, Quality = 0 });

        app.UpdateQuality();

        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void ConjuredItem_LosesQualityTwiceAsFast()
    {
        var app = CreateApp(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 });

        app.UpdateQuality();

        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(4, app.Items[0].Quality);
    }

    [Fact]
    public void ConjuredItem_LosesEvenMoreAfterSellByDate()
    {
        var app = CreateApp(new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(6, app.Items[0].Quality);
    }

    [Fact]
    public void ConjuredItem_QualityNeverGoesBelowZero()
    {
        var app = CreateApp(new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 1 });

        app.UpdateQuality();

        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_QualityGoesUp()
    {
        var app = CreateApp(new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 });

        app.UpdateQuality();

        Assert.Equal(1, app.Items[0].SellIn);
        Assert.Equal(1, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_QualityGoesUpFasterAfterSellByDate()
    {
        var app = CreateApp(new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(12, app.Items[0].Quality);
    }

    [Fact]
    public void AgedBrie_QualityNeverGoesAboveFifty()
    {
        var app = CreateApp(new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 });

        app.UpdateQuality();

        Assert.Equal(50, app.Items[0].Quality);
    }

    [Fact]
    public void Sulfuras_DoesNotChange()
    {
        var app = CreateApp(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 });

        app.UpdateQuality();

        Assert.Equal(0, app.Items[0].SellIn);
        Assert.Equal(80, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_IncreasesByTwoWhenTenDaysOrLess()
    {
        var app = CreateApp(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 10,
            Quality = 20
        });

        app.UpdateQuality();

        Assert.Equal(9, app.Items[0].SellIn);
        Assert.Equal(22, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_IncreasesByThreeWhenFiveDaysOrLess()
    {
        var app = CreateApp(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 5,
            Quality = 20
        });

        app.UpdateQuality();

        Assert.Equal(4, app.Items[0].SellIn);
        Assert.Equal(23, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_DropsToZeroAfterConcert()
    {
        var app = CreateApp(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 0,
            Quality = 20
        });

        app.UpdateQuality();

        Assert.Equal(-1, app.Items[0].SellIn);
        Assert.Equal(0, app.Items[0].Quality);
    }

    [Fact]
    public void BackstagePass_QualityNeverGoesAboveFifty()
    {
        var app = CreateApp(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 5,
            Quality = 49
        });

        app.UpdateQuality();

        Assert.Equal(50, app.Items[0].Quality);
    }
}
