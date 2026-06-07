using Xunit;
using GildedRose.Console;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    [Fact]
    public void ConjuredItemsDegradeQualityTwiceAsFastAsNormalItems()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(4, app.Items[0].Quality); // 6 - 2 = 4 (degraded twice as fast)
        Assert.Equal(2, app.Items[0].SellIn);   // 3 - 1 = 2 (sell-in decreases normally)
    }

    [Fact]
    public void ConjuredItemsDegradeQualityFourTimesAsFastAfterSellDate()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 8 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(4, app.Items[0].Quality); // 8 - 4 = 4 (degraded 4x after expiry)
        Assert.Equal(-1, app.Items[0].SellIn);  // 0 - 1 = -1
    }

    [Fact]
    public void ConjuredItemsQualityNeverGoesNegative()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = -1, Quality = 3 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(0, app.Items[0].Quality); // Would be -1, but capped at 0
    }

    [Fact]
    public void NormalItemsDegradeQualityByOne()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(19, app.Items[0].Quality); // 20 - 1 = 19
        Assert.Equal(9, app.Items[0].SellIn);    // 10 - 1 = 9
    }

    [Fact]
    public void AgedBrieIncreasesInQuality()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(1, app.Items[0].Quality); // 0 + 1 = 1
        Assert.Equal(1, app.Items[0].SellIn);   // 2 - 1 = 1
    }

    [Fact]
    public void SulfurasNeverChanges()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(80, app.Items[0].Quality); // Never changes
        Assert.Equal(0, app.Items[0].SellIn);    // Never changes
    }

    [Fact]
    public void BackstagePassesIncreaseInQualityBeforeExpiry()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(21, app.Items[0].Quality); // +1
        Assert.Equal(14, app.Items[0].SellIn);   // 15 - 1 = 14
    }

    [Fact]
    public void BackstagePassesIncreaseBy2When10DaysOrLess()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(22, app.Items[0].Quality); // +2
        Assert.Equal(9, app.Items[0].SellIn);    // 10 - 1 = 9
    }

    [Fact]
    public void BackstagePassesIncreaseBy3When5DaysOrLess()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(23, app.Items[0].Quality); // +3
        Assert.Equal(4, app.Items[0].SellIn);    // 5 - 1 = 4
    }

    [Fact]
    public void BackstagePassesDropTo0AfterConcert()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = -1, Quality = 50 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(0, app.Items[0].Quality); // Drops to 0
        Assert.Equal(-2, app.Items[0].SellIn);  // -1 - 1 = -2
    }

    [Fact]
    public void QualityNeverExceeds50()
    {
        // Arrange
        var app = new Program
        {
            Items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 2, Quality = 50 }
            }
        };

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(50, app.Items[0].Quality); // Capped at 50
    }
}