using System;
using UnityEngine;
using ModularInventory.Core;
using ModularInventory.Inventory;

namespace ModularInventory.Equipment
{
    [Serializable]
    public class EquipmentSlot
    {
        [field: SerializeField] public EquipmentType AllowedType { get; private set; }
        [field: SerializeField] public InventoryItem CurrentItem { get; private set; }

        public event Action<EquipmentSlot> OnEquipmentUpdated;

        public EquipmentSlot(EquipmentType type)
        {
            this.AllowedType = type;
        }

        public bool CanEquip(InventoryItem item)
        {
            if (item == null) return true;
            return item.Data.ItemType == ItemType.Equipment && item.Data.EquipmentType == AllowedType;
        }

        public bool Equip(InventoryItem item)
        {
            if (!CanEquip(item)) return false;

            CurrentItem = item;
            OnEquipmentUpdated?.Invoke(this);
            return true;
        }

        public InventoryItem Unequip()
        {
            InventoryItem item = CurrentItem;
            CurrentItem = null;
            OnEquipmentUpdated?.Invoke(this);
            return item;
        }
    }
}
