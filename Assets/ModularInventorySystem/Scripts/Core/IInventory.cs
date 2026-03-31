using System.Collections.Generic;
using ModularInventory.Data;
using ModularInventory.Inventory;

namespace ModularInventory.Core
{
    /// <summary>
    /// Contract for any inventory implementation.
    /// Allows systems to rely on abstraction rather than concrete implementations.
    /// </summary>
    public interface IInventory
    {
        /// <summary>
        /// Adds an item to the inventory. Returns true if fully or partially added.
        /// </summary>
        bool AddItem(ItemData data, int amount);

        /// <summary>
        /// Removes amount of an item from the inventory. Returns true if successful.
        /// </summary>
        bool RemoveItem(ItemData data, int amount);

        /// <summary>
        /// Checks if the inventory contains at least one of the item.
        /// </summary>
        bool HasItem(ItemData data);

        /// <summary>
        /// Returns the total stack count of a specific item inside the inventory.
        /// </summary>
        int GetItemCount(ItemData data);

        /// <summary>
        /// Clears all slots in the inventory.
        /// </summary>
        void ClearInventory();

        /// <summary>
        /// Retrieves all slots to display or analyze.
        /// </summary>
        List<InventorySlot> GetAllSlots();
    }
}
