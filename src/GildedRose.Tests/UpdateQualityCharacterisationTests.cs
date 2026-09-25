using Xunit;

namespace GildedRose.Tests;

/// <summary>
/// Characterisation tests: these pin the behaviour of the legacy UpdateQuality method
/// EXACTLY as it is today, before any refactoring. They are the safety net that proves
/// the refactor changed structure and nothing else.
///
/// They describe what the code does, not what we might wish it did.
/// </summary>
public class UpdateQualityCharacterisationTests
{
    // ---------------------------------------------------------------
    // Ordinary items
    // ---------------------------------------------------------------

    [Fact]
    public void NormalItem_BeforeSellByDate_LosesOneQualityPerDay()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Normal, sellIn: 10, quality: 20);

        Assert.Equal(9, item.SellIn);
        Assert.Equal(19, item.Quality);
    }

    [Fact]
    public void NormalItem_OnceSellByDateHasPassed_LosesTwoQualityPerDay()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Normal, sellIn: 0, quality: 10);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(8, item.Quality);
    }

    [Theory]
    [InlineData(5, 0)]   // already at the floor, before sell by
    [InlineData(0, 1)]   // would go negative via the double decrement
    [InlineData(0, 0)]   // already at the floor, after sell by
    [InlineData(-5, 1)]  // long expired
    public void NormalItem_QualityNeverGoesNegative(int sellIn, int quality)
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Normal, sellIn, quality);

        Assert.True(item.Quality >= 0, $"Quality was {item.Quality}");
    }

    [Fact]
    public void NormalItem_SellInKeepsCountingDownAfterExpiry()
    {
        var item = InventoryFixture.AfterDays(3, ItemNames.Normal, sellIn: 1, quality: 50);

        Assert.Equal(-2, item.SellIn);
        // day 1: sellIn 1 -> 0, quality 49 (not yet expired at the time of the check)
        // day 2: sellIn 0 -> -1, expired, quality 47
        // day 3: sellIn -1 -> -2, expired, quality 45
        Assert.Equal(45, item.Quality);
    }

    // ---------------------------------------------------------------
    // Aged Brie
    // ---------------------------------------------------------------

    [Fact]
    public void AgedBrie_IncreasesInQualityAsItAges()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.AgedBrie, sellIn: 2, quality: 0);

        Assert.Equal(1, item.SellIn);
        Assert.Equal(1, item.Quality);
    }

    [Fact]
    public void AgedBrie_OnceSellByDateHasPassed_GainsTwoQualityPerDay()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.AgedBrie, sellIn: 0, quality: 10);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(12, item.Quality);
    }

    [Theory]
    [InlineData(5, 50)]   // at the cap, before sell by
    [InlineData(-5, 50)]  // at the cap, after sell by
    [InlineData(0, 49)]   // the post-expiry double increment must not overshoot
    public void AgedBrie_QualityNeverExceedsFifty(int sellIn, int quality)
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.AgedBrie, sellIn, quality);

        Assert.Equal(50, item.Quality);
    }

    // ---------------------------------------------------------------
    // Sulfuras (legendary)
    // ---------------------------------------------------------------

    [Theory]
    [InlineData(5)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Sulfuras_NeverChangesQualityAndNeverHasToBeSold(int sellIn)
    {
        var item = InventoryFixture.AfterDays(10, ItemNames.Sulfuras, sellIn, quality: 80);

        Assert.Equal(sellIn, item.SellIn);
        Assert.Equal(80, item.Quality);
    }

    // ---------------------------------------------------------------
    // Backstage passes
    // ---------------------------------------------------------------

    [Theory]
    [InlineData(15, 21)]  // more than 10 days out: +1
    [InlineData(12, 21)]
    [InlineData(11, 21)]  // boundary: 11 days out is still +1
    [InlineData(10, 22)]  // 10 days or less: +2
    [InlineData(6, 22)]   // boundary: 6 days out is still +2
    [InlineData(5, 23)]   // 5 days or less: +3
    [InlineData(1, 23)]
    public void BackstagePass_GainsQualityFasterAsTheConcertApproaches(int sellIn, int expectedQuality)
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.BackstagePass, sellIn, quality: 20);

        Assert.Equal(sellIn - 1, item.SellIn);
        Assert.Equal(expectedQuality, item.Quality);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void BackstagePass_IsWorthlessOnceTheConcertHasPassed(int sellIn)
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.BackstagePass, sellIn, quality: 20);

        Assert.Equal(0, item.Quality);
    }

    [Theory]
    [InlineData(10, 50)]  // already at the cap
    [InlineData(5, 48)]   // +3 would overshoot; the cap is applied per increment
    [InlineData(5, 49)]
    [InlineData(10, 49)]
    public void BackstagePass_QualityNeverExceedsFifty(int sellIn, int quality)
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.BackstagePass, sellIn, quality);

        Assert.Equal(50, item.Quality);
    }

    [Fact]
    public void BackstagePass_FullLifecycleUpToAndThroughTheConcert()
    {
        // Twelve days takes the pass to the eve of the concert.
        var eveOfConcert = InventoryFixture.AfterDays(12, ItemNames.BackstagePass, sellIn: 12, quality: 20);
        Assert.Equal(0, eveOfConcert.SellIn);
        Assert.Equal(47, eveOfConcert.Quality);

        // The thirteenth day is the concert itself: the pass becomes worthless.
        var afterConcert = InventoryFixture.AfterDays(13, ItemNames.BackstagePass, sellIn: 12, quality: 20);
        Assert.Equal(-1, afterConcert.SellIn);
        Assert.Equal(0, afterConcert.Quality);
    }

    // ---------------------------------------------------------------
    // Conjured items
    // ---------------------------------------------------------------

    /// <summary>
    /// LEGACY BEHAVIOUR, PINNED DELIBERATELY. The inventory already contains a conjured
    /// item but the system has no concept of one, so it degrades like an ordinary item.
    /// This is the behaviour the new feature is required to change; the test is replaced
    /// in the commit that introduces conjured item support.
    /// </summary>
    [Fact]
    public void Legacy_ConjuredItem_IsCurrentlyTreatedAsAnOrdinaryItem()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Conjured, sellIn: 3, quality: 6);

        Assert.Equal(2, item.SellIn);
        Assert.Equal(5, item.Quality);
    }
}
