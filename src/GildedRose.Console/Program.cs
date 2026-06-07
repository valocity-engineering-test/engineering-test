using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    static void Main(string[] args)
    {
        System.Console.WriteLine("OMGHAI!");

        var app = new Program()
                      {
                          Items = new List<Item>
                                      {
                                          new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                          new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                          new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                          new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                          new Item
                                              {
                                                  Name = "Backstage passes to a TAFKAL80ETC concert",
                                                  SellIn = 15,
                                                  Quality = 20
                                              },
                                          new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                      }

                      };

        app.UpdateQuality();

        System.Console.ReadKey();
    }

    public void UpdateQuality()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            var itemUpdater = ItemUpdaterFactory.CreateUpdater(Items[i]);
            itemUpdater.Update(Items[i]);
        }
    }
}

/// <summary>
/// Factory to create the appropriate updater based on item type
/// </summary>
public static class ItemUpdaterFactory
{
    public static IItemUpdater CreateUpdater(Item item)
    {
        return item.Name switch
        {
            "Aged Brie" => new AgedBrieUpdater(),
            "Backstage passes to a TAFKAL80ETC concert" => new BackstagePassUpdater(),
            "Sulfuras, Hand of Ragnaros" => new SulfurasUpdater(),
            "Conjured Mana Cake" => new ConjuredItemUpdater(),
            _ => new NormalItemUpdater()
        };
    }
}

/// <summary>
/// Interface for item quality updaters
/// </summary>
public interface IItemUpdater
{
    void Update(Item item);
}

/// <summary>
/// Updater for normal items (e.g., +5 Dexterity Vest, Elixir of the Mongoose)
/// Degrades quality by 1 before expiry, by 2 after expiry
/// </summary>
public class NormalItemUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        DecreaseQuality(item, 1);
        DecreaseSellIn(item);

        if (item.SellIn < 0)
        {
            DecreaseQuality(item, 1);
        }
    }

    protected void DecreaseQuality(Item item, int amount)
    {
        item.Quality = System.Math.Max(0, item.Quality - amount);
    }

    protected void DecreaseSellIn(Item item)
    {
        item.SellIn--;
    }
}

/// <summary>
/// Updater for Conjured items
/// Degrades quality by 2 before expiry, by 4 after expiry (twice as fast as normal)
/// </summary>
public class ConjuredItemUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        DecreaseQuality(item, 2);  // Conjured degrades twice as fast
        DecreaseSellIn(item);

        if (item.SellIn < 0)
        {
            DecreaseQuality(item, 2);  // Conjured degrades twice as fast after expiry
        }
    }

    private void DecreaseQuality(Item item, int amount)
    {
        item.Quality = System.Math.Max(0, item.Quality - amount);
    }

    private void DecreaseSellIn(Item item)
    {
        item.SellIn--;
    }
}

/// <summary>
/// Updater for Aged Brie
/// Increases quality by 1 before expiry, by 2 after expiry
/// Quality never exceeds 50
/// </summary>
public class AgedBrieUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        IncreaseQuality(item, 1);
        DecreaseSellIn(item);

        if (item.SellIn < 0)
        {
            IncreaseQuality(item, 1);
        }
    }

    protected void IncreaseQuality(Item item, int amount)
    {
        item.Quality = System.Math.Min(50, item.Quality + amount);
    }

    protected void DecreaseSellIn(Item item)
    {
        item.SellIn--;
    }
}

/// <summary>
/// Updater for Backstage passes
/// Increases quality by 1 normally, +2 when less than 11 days, +3 when less than 6 days
/// Quality drops to 0 after the concert (SellIn < 0)
/// </summary>
public class BackstagePassUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        IncreaseQuality(item, 1);

        if (item.SellIn < 11)
        {
            IncreaseQuality(item, 1);
        }

        if (item.SellIn < 6)
        {
            IncreaseQuality(item, 1);
        }

        DecreaseSellIn(item);

        if (item.SellIn < 0)
        {
            item.Quality = 0;
        }
    }

    protected void IncreaseQuality(Item item, int amount)
    {
        item.Quality = System.Math.Min(50, item.Quality + amount);
    }

    protected void DecreaseSellIn(Item item)
    {
        item.SellIn--;
    }
}

/// <summary>
/// Updater for Sulfuras (Legendary item)
/// Never changes in quality or sell-in value
/// </summary>
public class SulfurasUpdater : IItemUpdater
{
    public void Update(Item item)
    {
        // Sulfuras never changes - do nothing
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
