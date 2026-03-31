using UnityEngine;

namespace ModularInventory.Core
{
    /// <summary>
    /// Defines the fundamental type or category of an item.
    /// </summary>
    public enum ItemType
    {
        None = 0,
        Equipment = 1,
        Consumable = 2,
        Material = 3,
        Quest = 4,
        Misc = 5
    }
}
