using System.Reflection;
using GameData;

namespace GTFO.AbsurdItems;

/// <summary>Helpers for building new items out of cloned vanilla datablocks.</summary>
internal static class DataBlocks
{
    // Copies every writable instance property onto a new block, so fields this mod doesn't
    // explicitly set start from the source item's values.
    internal static T Clone<T>(T source) where T : GameDataBlockBase<T>, new()
    {
        var copy = new T();
        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || !property.CanWrite || property.GetIndexParameters().Length > 0) continue;
            property.SetValue(copy, property.GetValue(source));
        }
        return copy;
    }
}
