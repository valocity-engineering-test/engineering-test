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
            UpdateItemQuality(item);
        }
    }

    private void UpdateItemQuality(Item item)
    {
        if (item.Name == "Sulfuras, Hand of Ragnaros") return;

        item.SellIn = item.SellIn - 1;

        switch (item.Name)
        {
            case "Aged Brie":
                IncreaseQuality(item);
                if (item.SellIn < 0) IncreaseQuality(item);
                break;

            case "Backstage passes to a TAFKAL80ETC concert":
                IncreaseQuality(item);
                if (item.SellIn < 10) IncreaseQuality(item);
                if (item.SellIn < 5) IncreaseQuality(item);
                if (item.SellIn < 0) item.Quality = 0;
                break;

            case "Conjured Mana Cake":
                DecreaseQuality(item, 2);
                if (item.SellIn < 0) DecreaseQuality(item, 2);
                break;

            default:
                DecreaseQuality(item, 1);
                if (item.SellIn < 0) DecreaseQuality(item, 1);
                break;
        }
    }
    private void IncreaseQuality(Item item)
    {
        if (item.Quality < 50)
        {
            item.Quality++;
        }
    }

    private void DecreaseQuality(Item item, int amount)
    {
        item.Quality -= amount;
        if (item.Quality < 0) item.Quality = 0;
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
