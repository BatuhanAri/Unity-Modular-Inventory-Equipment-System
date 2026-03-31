using System;
using System.Collections.Generic;
using UnityEngine;
using ModularInventory.Core;
using ModularInventory.Inventory;

namespace ModularInventory.Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<EquipmentType> availableSlots = new List<EquipmentType>()
        {
            EquipmentType.Head, EquipmentType.Chest, EquipmentType.Legs,
            EquipmentType.Feet, EquipmentType.Hands, EquipmentType.Weapon,
            EquipmentType.Shield, EquipmentType.Accessory
        };

        [Header("State")]
        [SerializeField] private List<EquipmentSlot> slots = new List<EquipmentSlot>();

        public event Action<InventoryItem, EquipmentType> OnItemEquipped;
        public event Action<InventoryItem, EquipmentType> OnItemUnequipped;

        private void Awake()
        {
            InitializeEquipmentSlots();
        }

        private void InitializeEquipmentSlots()
        {
            slots.Clear();
            foreach (var type in availableSlots)
            {
                slots.Add(new EquipmentSlot(type));
            }
        }

        public EquipmentSlot GetSlot(EquipmentType type)
        {
            return slots.Find(s => s.AllowedType == type);
        }

        public bool Equip(InventoryItem item, InventoryManager inventory)
        {
            if (item == null || item.Data.ItemType != ItemType.Equipment) return false;

            EquipmentSlot slot = GetSlot(item.Data.EquipmentType);
            if (slot == null) return false;

            // Take a copy of the item before we remove it to keep RuntimeID intact
            InventoryItem itemToEquip = item.Clone(1);

            // Remove from inventory
            if (!inventory.RemoveItem(item.Data, 1)) return false;

            // Unequip currently equipped item first
            InventoryItem previousItem = null;
            if (slot.CurrentItem != null)
            {
                previousItem = slot.Unequip();
            }

            // Re-add previous item to inventory
            if (previousItem != null)
            {
                inventory.AddItem(previousItem.Data, 1); // Note: Could add an AddItem specific for preserving RuntimeID in future
                OnItemUnequipped?.Invoke(previousItem, slot.AllowedType);
            }

            // Equip new item
            if (slot.Equip(itemToEquip))
            {
                OnItemEquipped?.Invoke(itemToEquip, slot.AllowedType);
                return true;
            }

            return false;
        }

        public bool Unequip(EquipmentType type, InventoryManager inventory)
        {
            EquipmentSlot slot = GetSlot(type);
            if (slot == null || slot.CurrentItem == null) return false;

            InventoryItem item = slot.CurrentItem;
            
            // Re-add to inventory
            if (inventory.AddItem(item.Data, 1))
            {
                slot.Unequip();
                OnItemUnequipped?.Invoke(item, type);
                return true;
            }

            return false; // Could not add to inventory (maybe full)
        }
        
        public List<EquipmentSlot> GetAllSlots() => slots;
    }
}
