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
    public void ConjuredItemsDegradeTwiceAsFastAsNormalItems()
    {
        var app = new GildedRose.Console.Program
        {
            Items =
            [
                new GildedRose.Console.Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
            ]
        };

        app.UpdateQuality();

        Assert.Equal(2, app.Items[0].SellIn);
        Assert.Equal(4, app.Items[0].Quality);
    }
}