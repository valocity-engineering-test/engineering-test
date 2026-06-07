namespace GildedRose.Console.Helpers;

public static class QualityHelper
{
    public static void Increase(Item item, int amount)
    {
        if (item.Quality + amount > 50)
        {
            item.Quality = 50;
        }
        else
        {
            item.Quality = item.Quality + amount;
        }
    }

    public static void Decrease(Item item, int amount)
    {
        if (item.Quality - amount < 0)
        {
            item.Quality = 0;
        }
        else
        {
            item.Quality = item.Quality - amount;
        }
    }
}
