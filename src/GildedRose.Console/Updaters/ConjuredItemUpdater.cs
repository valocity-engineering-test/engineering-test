namespace GildedRose.Console.Updaters;

/// <summary>
/// Conjured goods are ordinary goods that degrade twice as fast.
/// </summary>
/// <remarks>
/// Expressed as a doubled degradation rate rather than as its own set of rules, so the
/// relationship to ordinary items stays visible and the behaviour either side of the
/// sell-by date stays consistent with it by construction.
/// </remarks>
public sealed class ConjuredItemUpdater : DefaultItemUpdater
{
    protected override int DegradationRate => 2;
}
