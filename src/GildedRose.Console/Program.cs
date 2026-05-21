using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    private IList<Item> Items = new List<Item>();

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

        var factory = new ItemUpdaterFactory();

        var inventoryManager = new InventoryManager(factory);

        inventoryManager.UpdateQuality(app.Items);

        System.Console.ReadKey();
    }

    public interface IItemUpdater
    {
        void Update(Item item);
    }

    public abstract class BaseItemUpdater : IItemUpdater
    {
        public abstract void Update(Item item);

        protected void IncreaseQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Min(50, item.Quality + amount);
        }

        protected void DecreaseQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Max(0, item.Quality - amount);
        }

        protected void DecreaseSellIn(Item item)
        {
            item.SellIn--;
        }
    }
    public class NormalItemUpdater : BaseItemUpdater
    {
        public override void Update(Item item)
        {
            DecreaseSellIn(item);

            int degrade = item.SellIn < 0 ? 2 : 1;

            DecreaseQuality(item, degrade);

        }
    }

    public class AgedBrieUpdater : BaseItemUpdater
    {
        public override void Update(Item item)
        {
            DecreaseSellIn(item);

            int increase = item.SellIn < 0 ? 2 : 1;

            IncreaseQuality(item, increase);
        }
    }

    public class BackstagePassUpdater : BaseItemUpdater
    {
        public override void Update(Item item)
        {
            DecreaseSellIn(item);

            if (item.SellIn < 0)
            {
                item.Quality = 0;
                return;
            }

            IncreaseQuality(item);

            if (item.SellIn < 10)
            {
                IncreaseQuality(item);
            }

            if (item.SellIn < 5)
            {
                IncreaseQuality(item);
            }
        }
    }

    public class SulfurasUpdater : BaseItemUpdater
    {
        public override void Update(Item item)
        {
            // Legendary item does not change
        }
    }

    public class ConjuredItemUpdater : BaseItemUpdater
    {
        public override void Update(Item item)
        {
            DecreaseSellIn(item);

            int degrade = item.SellIn < 0 ? 4 : 2;

            DecreaseQuality(item, degrade);
        }
    }

    public class ItemUpdaterFactory
    {
        private readonly Dictionary<string, IItemUpdater> _updaters;

        public ItemUpdaterFactory()
        {
            _updaters = new Dictionary<string, IItemUpdater>
            {
                { "Aged Brie", new AgedBrieUpdater() },
                { "Backstage passes to a TAFKAL80ETC concert", new BackstagePassUpdater() },
                { "Sulfuras, Hand of Ragnaros", new SulfurasUpdater() },
                { "Conjured Mana Cake", new ConjuredItemUpdater() }
            };
        }

        public IItemUpdater GetUpdater(Item item)
        {
            return _updaters.ContainsKey(item.Name)
                ? _updaters[item.Name]
                : new NormalItemUpdater();
        }
    }

    public class InventoryManager
    {
        private readonly ItemUpdaterFactory _factory;

        public InventoryManager(ItemUpdaterFactory factory)
        {
            _factory = factory;
        }

        public void UpdateQuality(IList<Item> items)
        {
            foreach (var item in items)
            {
                var updater = _factory.GetUpdater(item);

                updater.Update(item);
            }
        }
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
