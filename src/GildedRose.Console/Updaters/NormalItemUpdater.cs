namespace GildedRose.Console.Updaters;

public class NormalItemUpdater : DegradingItemUpdater
{
    protected override int DegradeRate => 1;
}
