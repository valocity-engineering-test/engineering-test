using GildedRose.Console;
using GildedRose.Console.Updaters;
using Xunit;

namespace GildedRose.Tests;

public class UpdaterTests
{
    [Fact]
    public void NormalItemUpdater_DecreasesQualityAndSellIn()
    {
        var item = new Item { Name = "Elixir", SellIn = 5, Quality = 10 };
        var updater = new NormalItemUpdater();

        updater.Update(item);

        Assert.Equal(4, item.SellIn);
        Assert.Equal(9, item.Quality);
    }

    [Fact]
    public void ConjuredItemUpdater_DecreasesQualityTwiceAsFast()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };
        var updater = new ConjuredItemUpdater();

        updater.Update(item);

        Assert.Equal(2, item.SellIn);
        Assert.Equal(4, item.Quality);
    }

    [Fact]
    public void AgedBrieUpdater_IncreasesQuality()
    {
        var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };
        var updater = new AgedBrieUpdater();

        updater.Update(item);

        Assert.Equal(1, item.SellIn);
        Assert.Equal(1, item.Quality);
    }

    [Fact]
    public void BackstagePassUpdater_IncreasesQualityBeforeConcert()
    {
        var item = new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 15,
            Quality = 20
        };
        var updater = new BackstagePassUpdater();

        updater.Update(item);

        Assert.Equal(14, item.SellIn);
        Assert.Equal(21, item.Quality);
    }

    [Fact]
    public void SulfurasUpdater_DoesNothing()
    {
        var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };
        var updater = new SulfurasUpdater();

        updater.Update(item);

        Assert.Equal(0, item.SellIn);
        Assert.Equal(80, item.Quality);
    }

    [Fact]
    public void ItemUpdaterFactory_ReturnsConjuredUpdaterForConjuredItems()
    {
        var item = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 };
        var updater = ItemUpdaterFactory.GetUpdater(item);

        updater.Update(item);

        Assert.Equal(2, item.SellIn);
        Assert.Equal(4, item.Quality);
    }

    [Fact]
    public void ItemUpdaterFactory_ReturnsNormalUpdaterForRegularItems()
    {
        var item = new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 };
        var updater = ItemUpdaterFactory.GetUpdater(item);

        updater.Update(item);

        Assert.Equal(9, item.SellIn);
        Assert.Equal(19, item.Quality);
    }
}
