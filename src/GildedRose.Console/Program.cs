using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    private const int MaxQuality = 50;
    private const int MinQuality = 0;

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
                                                  SellIn = 5,
                                                  Quality = 20
                                              },
                                          new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6},
                                          new Item {Name = "Conjured Items1", SellIn = 5, Quality = 10},
                                          new Item {Name = "Conjured Items2", SellIn = 0, Quality = 8},
                                          new Item {Name = "Conjured Items3", SellIn = 3, Quality = 1}
                                      }

                      };

        app.UpdateQuality();

        System.Console.ReadKey();
    }


    public void UpdateQuality()
    {
        foreach(var item in Items)
        {
            if(item.Name.Equals("Sulfuras, Hand of Ragnaros", StringComparison.Ordinal))
            {
                continue;
            }

            if(item.Name.Equals("Backstage passes to a TAFKAL80ETC concert", StringComparison.Ordinal))
            {
                UpdateBackstage(item);
            }
            else if(item.Name.Equals("Aged Brie", StringComparison.Ordinal))
            {
                UpdateAgeBrie(item);
            }
            else
            {
                UpdateNormalItems(item);
            }

            item.SellIn--;

            if(item.SellIn < 0)
            {
                UpdateAfterSellInPassed(item);
            }
         
            //chekcing output
            System.Console.WriteLine($"Item Name: {item.Name}, Sellin: {item.SellIn}, Quality: {item.Quality}");
        }
        
    }
    private void UpdateBackstage(Item item)
    {
        IncreaseQuality(item);
        if (item.SellIn <= 10)
        {
            IncreaseQuality(item);
        }
        if (item.SellIn <= 5)
        {
            IncreaseQuality(item);
        }
    }
    private static void UpdateAgeBrie(Item item)
    {
        IncreaseQuality(item);
    }
    private static void UpdateNormalItems(Item item)
    {
        DecreaseQuality(item);
    }
    private static void UpdateAfterSellInPassed(Item item)
    {
        if (item.Name.Equals("Backstage passes to a TAFKAL80ETC concert", StringComparison.Ordinal))
        {
            item.Quality = MinQuality;
        }
        if (item.Name.Equals("Aged Brie", StringComparison.Ordinal))
        {
            IncreaseQuality(item);
        }
        DecreaseQuality(item);
    }
    private static void IncreaseQuality(Item item)
    {
        item.Quality = item.Quality + 1 > MaxQuality ? MaxQuality : item.Quality + 1;
    }
    private static void DecreaseQuality(Item item)
    {
        if (item.Name.StartsWith("Conjured", StringComparison.Ordinal))
        {
            item.Quality = item.Quality - 2 < MinQuality ? MinQuality : item.Quality - 2;
        }
        else
        {
            item.Quality = item.Quality - 1 < MinQuality ? MinQuality : item.Quality - 1;
        }
            
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
