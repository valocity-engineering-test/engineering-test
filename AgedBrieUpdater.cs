namespace GildedRose
{
    public class AgedBrieUpdater : NormalItemUpdater
    {
        public override void Update(Item item)
        {
            IncreaseQuality(item, 1);

            item.SellIn--;

            if (item.SellIn < 0)
            {
                IncreaseQuality(item, 1);
            }
        }
    }
}