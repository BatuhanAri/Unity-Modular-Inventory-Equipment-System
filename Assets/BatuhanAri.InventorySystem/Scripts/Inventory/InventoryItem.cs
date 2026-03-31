using UnityEngine;
using BatuhanAri.InventorySystem.Data;
using System;

namespace BatuhanAri.InventorySystem.Inventory
{
    /// <summary>
    /// Represents the runtime instance of an item in the inventory.
    /// Manages the dynamic properties like Stack Count and Runtime ID.
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private ItemData data;
        [field: SerializeField] public int CurrentStack { get; private set; }
        public string RuntimeID { get; private set; }

        public ItemData Data => data;

        /// <summary>
        /// Creates a new runtime item referencing the provided ItemData.
        /// </summary>
        public InventoryItem(ItemData itemData, int stackCount)
        {
            this.data = itemData;
            this.CurrentStack = stackCount;
            this.RuntimeID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Attempts to add to the stack. Returns the leftover amount if stack limit is exceeded.
        /// </summary>
        public int AddToStack(int amount)
        {
            int total = CurrentStack + amount;
            if (total > data.MaxStack)
            {
                CurrentStack = data.MaxStack;
                return total - data.MaxStack;
            }
            CurrentStack = total;
            return 0;
        }

        /// <summary>
        /// Removes from the stack.
        /// </summary>
        public void RemoveFromStack(int amount)
        {
            CurrentStack -= amount;
            if (CurrentStack < 0) CurrentStack = 0;
        }

        /// <summary>
        /// Creates a carbon copy of this item, typically used when transferring between different systems (like Equipment).
        /// </summary>
        public InventoryItem Clone(int newStackCount)
        {
            InventoryItem clone = new InventoryItem(this.data, newStackCount);
            clone.RuntimeID = this.RuntimeID; // Preserve the unique identity
            return clone;
        }
    }
}

