
using GildedRose;
using Xunit;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    [Fact]
    public void TestTheTruth()
    {
        Assert.True(true);
    }

    [Fact]
    public void AgedBrie_Should_Increase_Quality()
    {
        var item = new Item
        {
            Name = "Aged Brie",
            SellIn = 5,
            Quality = 10
        };

        var updater = new AgedBrieUpdater();

        updater.Update(item);

        Assert.Equal(11, item.Quality);
    }

    [Fact]
    public void Conjured_Item_Should_Decrease_Quality_By_2()
    {
        var item = new Item
        {
            Name = "Conjured Mana Cake",
            SellIn = 5,
            Quality = 10
        };

        var updater = new ConjuredItemUpdater();

        updater.Update(item);

        Assert.Equal(8, item.Quality);
    }
}