namespace GildedRose
{
    public class BackstagePassUpdater : NormalItemUpdater
    {
        public override void Update(Item item)
        {
            IncreaseQuality(item, 1);

            if (item.SellIn <= 10)
            {
                IncreaseQuality(item, 1);
            }

            if (item.SellIn <= 5)
            {
                IncreaseQuality(item, 1);
            }

            item.SellIn--;

            if (item.SellIn < 0)
            {
                item.Quality = 0;
            }
        }
    }
}