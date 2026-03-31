using System;
using UnityEngine;

namespace BatuhanAri.InventorySystem.Inventory
{
    /// <summary>
    /// Represents a single slot in the inventory. Contains an InventoryItem and notifies changes.
    /// </summary>
    [Serializable]
    public class InventorySlot
    {
        [field: SerializeField] public InventoryItem Item { get; private set; }
        [field: SerializeField] public int SlotIndex { get; private set; }

        public bool IsEmpty => Item == null || Item.CurrentStack <= 0;

        public event Action<InventorySlot> OnSlotUpdated;

        public InventorySlot(int slotIndex)
        {
            this.SlotIndex = slotIndex;
            ClearSlot();
        }

        public void SetItem(InventoryItem item)
        {
            this.Item = item;
            OnSlotUpdated?.Invoke(this);
        }

        public void ClearSlot()
        {
            this.Item = null;
            OnSlotUpdated?.Invoke(this);
        }

        public void RefreshSlot()
        {
            if (Item != null && Item.CurrentStack <= 0)
            {
                ClearSlot();
            }
            else
            {
                OnSlotUpdated?.Invoke(this);
            }
        }
    }
}

