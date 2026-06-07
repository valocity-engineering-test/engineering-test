using GildedRose.Console;
using GildedRose.Console.Helpers;
using Xunit;

namespace GildedRose.Tests;

public class QualityHelperTests
{
    [Fact]
    public void Increase_DoesNotGoAboveFifty()
    {
        var item = new Item { Name = "test", SellIn = 5, Quality = 48 };

        QualityHelper.Increase(item, 5);

        Assert.Equal(50, item.Quality);
    }

    [Fact]
    public void Decrease_DoesNotGoBelowZero()
    {
        var item = new Item { Name = "test", SellIn = 5, Quality = 2 };

        QualityHelper.Decrease(item, 5);

        Assert.Equal(0, item.Quality);
    }
}
