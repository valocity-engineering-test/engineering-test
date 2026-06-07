using GildedRose.Console;
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
    public void ConjuredItems_DegradeTwiceAsFast()
    {
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 4 },
                new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7}
            }
        };

        app.UpdateQuality();

        Assert.Equal(2, app.Items[0].Quality);
        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(6, app.Items[1].Quality);
        Assert.Equal(4, app.Items[1].SellIn);
    }
}