namespace GildedRose
{
    public class ConjuredItemUpdater : NormalItemUpdater
    {
        public override void Update(Item item)
        {
            DecreaseQuality(item, 2);

            item.SellIn--;

            if (item.SellIn < 0)
            {
                DecreaseQuality(item, 2);
            }
        }
    }
}