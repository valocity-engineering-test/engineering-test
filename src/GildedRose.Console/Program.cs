using GildedRose.Console.Factories;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    private readonly ItemQualityUpdaterFactory _qualityUpdaterFactory;

    public Program()
    {
        _qualityUpdaterFactory = new ItemQualityUpdaterFactory();
    }

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
            var qualityUpdater = _qualityUpdaterFactory.GetUpdater(item);
            qualityUpdater.Update(item);
        }
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}

public static class ItemNames
{
    public const string AgedBrie = "Aged Brie";

    public const string Sulfuras = "Sulfuras, Hand of Ragnaros";

    public const string BackstagePass = "Backstage passes to a TAFKAL80ETC concert";

    public const string Conjured = "Conjured";
}