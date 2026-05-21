using GildedRose.Console;
using Xunit;
using System.Collections.Generic;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    [Fact]
    public void TestTheTruth()
    {
        Assert.True(true);
    }

    [Fact]
    public void NormalItem_Degrades_By_One_Before_SellBy()
    {
        var items = new List<Item>
        {
            new Item { Name = "+5 Dexterity Vest", SellIn = 5, Quality = 10 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(4, items[0].SellIn);
        Assert.Equal(9, items[0].Quality);
    }

    [Fact]
    public void NormalItem_Degrades_Twice_After_SellBy()
    {
        var items = new List<Item>
        {
            new Item { Name = "+5 Dexterity Vest", SellIn = 0, Quality = 10 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(-1, items[0].SellIn);
        Assert.Equal(8, items[0].Quality);
    }

    [Fact]
    public void Quality_Is_Never_Negative()
    {
        var items = new List<Item>
        {
            new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 0 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(0, items[0].Quality);
    }

    [Fact]
    public void AgedBrie_Increases_In_Quality()
    {
        var items = new List<Item>
        {
            new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(1, items[0].SellIn);
        Assert.Equal(1, items[0].Quality);
    }

    [Fact]
    public void AgedBrie_Increases_Twice_After_SellBy()
    {
        var items = new List<Item>
        {
            new Item { Name = "Aged Brie", SellIn = 0, Quality = 0 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(-1, items[0].SellIn);
        Assert.Equal(2, items[0].Quality);
    }

    [Fact]
    public void AgedBrie_Quality_Is_Capped_At_50()
    {
        var items = new List<Item>
        {
            new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(50, items[0].Quality);
    }

    [Fact]
    public void Sulfuras_Does_Not_Change()
    {
        var items = new List<Item>
        {
            new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 5, Quality = 80 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(5, items[0].SellIn);
        Assert.Equal(80, items[0].Quality);
    }

    [Fact]
    public void Backstage_Increases_By_One_When_More_Than_10_Days()
    {
        var items = new List<Item>
        {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 12, Quality = 20 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(11, items[0].SellIn);
        Assert.Equal(21, items[0].Quality);
    }

    [Fact]
    public void Backstage_Increases_By_Two_When_10_Days_Or_Less()
    {
        var items = new List<Item>
        {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(9, items[0].SellIn);
        Assert.Equal(22, items[0].Quality);
    }

    [Fact]
    public void Backstage_Increases_By_Three_When_5_Days_Or_Less()
    {
        var items = new List<Item>
        {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(4, items[0].SellIn);
        Assert.Equal(23, items[0].Quality);
    }

    [Fact]
    public void Backstage_Drops_To_Zero_After_Concert()
    {
        var items = new List<Item>
        {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 20 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(-1, items[0].SellIn);
        Assert.Equal(0, items[0].Quality);
    }

    [Fact]
    public void Backstage_Quality_Is_Capped_At_50()
    {
        var items = new List<Item>
        {
            new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 49 }
        };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(50, items[0].Quality);
    }

    [Fact]
    public void Conjured_Degrades_Twice_As_Fast_Before_SellBy()
    {
        var items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 10 }
            };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(4, items[0].SellIn);
        Assert.Equal(8, items[0].Quality);
    }

    [Fact]
    public void Conjured_Degrades_Twice_As_Fast_After_SellBy()
    {
        var items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 }
            };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(-1, items[0].SellIn);
        Assert.Equal(6, items[0].Quality); // degrades by 4 after sell by
    }

    [Fact]
    public void Conjured_Quality_Is_Never_Negative()
    {
        var items = new List<Item>
            {
                new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 1 }
            };

        var factory = new Program.ItemUpdaterFactory();
        var manager = new Program.InventoryManager(factory);

        manager.UpdateQuality(items);

        Assert.Equal(0, items[0].Quality);
    }
}