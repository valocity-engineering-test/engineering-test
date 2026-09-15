using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    private const string AgedBrie = "Aged Brie";
    private const string BackstagePass = "Backstage passes to a TAFKAL80ETC concert";
    private const string Sulfuras = "Sulfuras, Hand of Ragnaros";
    private const int MaximumQuality = 50;

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
            if (item.Name == Sulfuras)
            {
                //"Sulfuras", being a legendary item, never has to be sold or decreases in Quality
                continue;
            }

            UpdateItemQuality(item);
            item.SellIn--;
        }
    }

    private static void UpdateItemQuality(Item item)
    {
        if (item.Name == AgedBrie)
        {
            //"Aged Brie" actually increases in Quality the older it gets
            IncreaseQuality(item, item.SellIn <= 0 ? 2 : 1);
            return;
        }

        if (item.Name == BackstagePass)
        {
            if (item.SellIn <= 0)
            {
                item.Quality = 0;
                return;
            }
            //"Backstage passes", like aged brie, increases in Quality as its SellIn value approaches:
            //Quality increases by 2 when there are 10 days or less
            //Quality increases by 3 when there are 5 days or less
            //Quality drops to 0 after the concert
            var increase = item.SellIn <= 5 ? 3 : item.SellIn <= 10 ? 2 : 1;
            IncreaseQuality(item, increase);
            return;
        }
        //"Conjured" items degrade in Quality twice as fast as normal items
        var degradation = item.Name.StartsWith("Conjured", StringComparison.Ordinal) ? 2 : 1;
        DecreaseQuality(item, item.SellIn <= 0 ? degradation * 2 : degradation);
    }

    private static void IncreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Min(MaximumQuality, item.Quality + amount);
    }

    private static void DecreaseQuality(Item item, int amount)
    {
        item.Quality = Math.Max(0, item.Quality - amount);
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
