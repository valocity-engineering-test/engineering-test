using Xunit;

namespace GildedRose.Tests;

/// <summary>
/// The new supplier's requirement: conjured items degrade in quality twice as fast as
/// ordinary items.
/// </summary>
public class ConjuredItemTests
{
    [Fact]
    public void ConjuredItem_BeforeSellByDate_LosesTwoQualityPerDay()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Conjured, sellIn: 3, quality: 6);

        Assert.Equal(2, item.SellIn);
        Assert.Equal(4, item.Quality);
    }

    [Fact]
    public void ConjuredItem_OnceSellByDateHasPassed_LosesFourQualityPerDay()
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Conjured, sellIn: 0, quality: 10);

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(6, item.Quality);
    }

    [Theory]
    [InlineData(5, 1)]   // would go negative before the sell-by date
    [InlineData(0, 3)]   // would go negative after it
    [InlineData(0, 0)]   // already at the floor
    public void ConjuredItem_QualityNeverGoesNegative(int sellIn, int quality)
    {
        var item = InventoryFixture.AfterOneDay(ItemNames.Conjured, sellIn, quality);

        Assert.Equal(0, item.Quality);
    }

    [Theory]
    [InlineData(10, 40)]  // before the sell-by date
    [InlineData(0, 40)]   // after it
    public void ConjuredItem_DegradesAtExactlyTwiceTheRateOfAnOrdinaryItem(int sellIn, int quality)
    {
        var ordinary = InventoryFixture.AfterOneDay(ItemNames.Normal, sellIn, quality);
        var conjured = InventoryFixture.AfterOneDay(ItemNames.Conjured, sellIn, quality);

        var ordinaryLoss = quality - ordinary.Quality;
        var conjuredLoss = quality - conjured.Quality;

        Assert.Equal(ordinaryLoss * 2, conjuredLoss);
    }

    /// <summary>
    /// "Conjured" is a category, not a single product, so the supplier is expected to
    /// ship more than one line of it. See SOLUTION.md for the reasoning behind matching
    /// on the prefix rather than on the one name currently in the inventory.
    /// </summary>
    [Theory]
    [InlineData("Conjured Mana Cake")]
    [InlineData("Conjured Sword")]
    [InlineData("Conjured Health Potion")]
    public void AnyConjuredItem_IsRecognisedByItsCategory(string name)
    {
        var item = InventoryFixture.AfterOneDay(name, sellIn: 5, quality: 20);

        Assert.Equal(18, item.Quality);
    }

    [Theory]
    [InlineData("Conjuration Robes")]      // shares a stem, but is not a conjured item
    [InlineData("Sword of the Conjured")]  // the word appears, but not as the category
    public void ItemsThatMerelyMentionConjuring_AgeAsOrdinaryItems(string name)
    {
        var item = InventoryFixture.AfterOneDay(name, sellIn: 5, quality: 20);

        Assert.Equal(19, item.Quality);
    }
}
