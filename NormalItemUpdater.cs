namespace GildedRose
{
    public class NormalItemUpdater : IItemUpdater
    {
        public virtual void Update(Item item)
        {
            DecreaseQuality(item, 1);

            item.SellIn--;

            if (item.SellIn < 0)
            {
                DecreaseQuality(item, 1);
            }
        }

        protected void DecreaseQuality(Item item, int amount)
        {
            item.Quality -= amount;

            if (item.Quality < 0)
            {
                item.Quality = 0;
            }
        }

        protected void IncreaseQuality(Item item, int amount)
        {
            item.Quality += amount;

            if (item.Quality > 50)
            {
                item.Quality = 50;
            }
        }
    }
}