namespace GildedRose.Console.Updaters;

/// <summary>
/// Applies one day's ageing to a single item.
/// </summary>
/// <remarks>
/// The rules vary along exactly one axis - the category of the item - and the set of
/// categories is open for extension. The behaviour therefore lives in collaborators that
/// act on an <see cref="Item"/>, rather than on the item itself, which is owned elsewhere
/// and must not be modified.
/// </remarks>
public interface IItemUpdater
{
    void Update(Item item);
}
