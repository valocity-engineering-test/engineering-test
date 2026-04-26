using GildedRoseApp.Console;  // reference the namespace where GildedRose lives
using System.Collections.Generic;
using Xunit;

namespace GildedRoseApp.Tests
{
    public class GildedRoseTests
    {
        private GildedRose CreateAppWithItem(Item item)
        {
            return new GildedRose(new List<Item> { item });
        }

        [Fact]
        public void NormalItem_QualityDecreasesBy1EachDay()
        {
            var app = CreateAppWithItem(new Item { Name = "Normal Item", SellIn = 10, Quality = 20 });

            app.UpdateQuality();

            Assert.Equal(19, app.Items[0].Quality);
            Assert.Equal(9, app.Items[0].SellIn);
        }

        [Fact]
        public void NormalItem_QualityDecreasesTwiceAsFastAfterSellIn()
        {
            var app = CreateAppWithItem(new Item { Name = "Normal Item", SellIn = 0, Quality = 10 });

            app.UpdateQuality();

            Assert.Equal(8, app.Items[0].Quality);
            Assert.Equal(-1, app.Items[0].SellIn);
        }

        [Fact]
        public void QualityNeverNegative()
        {
            var app = CreateAppWithItem(new Item { Name = "Normal Item", SellIn = 5, Quality = 0 });

            app.UpdateQuality();

            Assert.Equal(0, app.Items[0].Quality);
        }

        [Fact]
        public void AgedBrie_IncreasesInQuality()
        {
            var app = CreateAppWithItem(new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 });

            app.UpdateQuality();

            Assert.Equal(1, app.Items[0].Quality);
            Assert.Equal(1, app.Items[0].SellIn);
        }

        [Fact]
        public void AgedBrie_QualityNeverExceeds50()
        {
            var app = CreateAppWithItem(new Item { Name = "Aged Brie", SellIn = 2, Quality = 50 });

            app.UpdateQuality();

            Assert.Equal(50, app.Items[0].Quality);
        }

        [Fact]
        public void Sulfuras_NeverChanges()
        {
            var app = CreateAppWithItem(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 });

            app.UpdateQuality();

            Assert.Equal(80, app.Items[0].Quality);
            Assert.Equal(0, app.Items[0].SellIn);
        }

        [Fact]
        public void BackstagePass_IncreasesBy1Normally()
        {
            var app = CreateAppWithItem(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 15, Quality = 20 });

            app.UpdateQuality();

            Assert.Equal(21, app.Items[0].Quality);
            Assert.Equal(14, app.Items[0].SellIn);
        }

        [Fact]
        public void BackstagePass_IncreasesBy2When10DaysOrLess()
        {
            var app = CreateAppWithItem(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 10, Quality = 20 });

            app.UpdateQuality();

            Assert.Equal(22, app.Items[0].Quality);
        }

        [Fact]
        public void BackstagePass_IncreasesBy3When5DaysOrLess()
        {
            var app = CreateAppWithItem(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 20 });

            app.UpdateQuality();

            Assert.Equal(23, app.Items[0].Quality);
        }

        [Fact]
        public void BackstagePass_DropsToZeroAfterConcert()
        {
            var app = CreateAppWithItem(new Item { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 0, Quality = 20 });

            app.UpdateQuality();

            Assert.Equal(0, app.Items[0].Quality);
        }

        [Fact]
        public void ConjuredItem_DegradesTwiceAsFast()
        {
            var app = CreateAppWithItem(new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 });

            app.UpdateQuality();

            Assert.Equal(4, app.Items[0].Quality);
            Assert.Equal(2, app.Items[0].SellIn);
        }

        [Fact]
        public void ConjuredItem_DegradesTwiceAsFastAfterSellIn()
        {
            var app = CreateAppWithItem(new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 6 });

            app.UpdateQuality();

            Assert.Equal(2, app.Items[0].Quality);
            Assert.Equal(-1, app.Items[0].SellIn);
        }
    }
}
