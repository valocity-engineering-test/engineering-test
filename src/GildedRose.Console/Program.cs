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
        foreach (var item in Items)
        {
            UpdateItem(item);
        }
    }

    private static void UpdateItem(Item item)
    {
        if (item.Name == "Sulfuras, Hand of Ragnaros")
        {
            return;
        }

        item.SellIn--;

        if (item.Name == "Aged Brie")
        {
            item.Quality = UpdateIncreasingQuality(item.Quality, 1, item.SellIn < 0 ? 2 : 0);
            return;
        }

        if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            if (item.SellIn < 0)
            {
                item.Quality = 0;
                return;
            }

            var qualityIncrease = 1;
            if (item.SellIn < 10)
            {
                qualityIncrease++;
            }

            if (item.SellIn < 5)
            {
                qualityIncrease++;
            }

            item.Quality = UpdateIncreasingQuality(item.Quality, qualityIncrease, 0);
            return;
        }

        var drain = IsConjured(item) ? 2 : 1;
        if (item.SellIn < 0)
        {
            drain *= 2;
        }

        item.Quality = ClampQuality(item.Quality - drain);
    }

    private static int UpdateIncreasingQuality(int currentQuality, int baseIncrease, int expiredIncrease)
    {
        var totalIncrease = baseIncrease + expiredIncrease;
        return ClampQuality(currentQuality + totalIncrease);
    }

    private static bool IsConjured(Item item)
    {
        return item.Name.StartsWith("Conjured", System.StringComparison.Ordinal);
    }

    private static int ClampQuality(int quality)
    {
        if (quality < 0)
        {
            return 0;
        }

        if (quality > 50)
        {
            return 50;
        }

        return quality;
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
