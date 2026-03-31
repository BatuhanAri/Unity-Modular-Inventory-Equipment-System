using System;
using System.Collections.Generic;
using UnityEngine;
using ModularInventory.Core;
using ModularInventory.Data;

namespace ModularInventory.Inventory
{
    /// <summary>
    /// Core logic for inventory management. Handles adding, removing, sorting and tracking items.
    /// Uses Observer pattern with events to decouple from UI.
    /// </summary>
    public class InventoryManager : MonoBehaviour, IInventory
    {
        [Header("Settings")]
        [SerializeField] private int initialMaxSlots = 20;

        [Header("State")]
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        public int MaxSlots => slots.Count;

        // Events
        public event Action<ItemData, int> OnItemAdded;
        public event Action<ItemData, int> OnItemRemoved;
        public event Action OnInventoryUpdated;

        private void Awake()
        {
            InitializeInventory(initialMaxSlots);
        }

        public void InitializeInventory(int size)
        {
            slots.Clear();
            for (int i = 0; i < size; i++)
            {
                slots.Add(new InventorySlot(i));
            }
        }

        public bool AddItem(ItemData data, int amount)
        {
            if (data == null || amount <= 0) return false;

            int remainingAmount = amount;

            // 1. Try to add to stackable slots first
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item.Data == data && slot.Item.CurrentStack < data.MaxStack)
                {
                    remainingAmount = slot.Item.AddToStack(remainingAmount);
                    slot.RefreshSlot();
                    
                    if (remainingAmount <= 0)
                    {
                        OnItemAdded?.Invoke(data, amount);
                        OnInventoryUpdated?.Invoke();
                        return true;
                    }
                }
            }

            // 2. Add to empty slots
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                {
                    int stackAmount = Mathf.Min(remainingAmount, data.MaxStack);
                    InventoryItem newItem = new InventoryItem(data, stackAmount);
                    slot.SetItem(newItem);

                    remainingAmount -= stackAmount;
                    
                    if (remainingAmount <= 0)
                    {
                        OnItemAdded?.Invoke(data, amount);
                        OnInventoryUpdated?.Invoke();
                        return true;
                    }
                }
            }

            // If we have remaining amount and no slots left, we failed to add it all.
            // Ideally we'd drop the rest on the floor here.
            OnInventoryUpdated?.Invoke();
            return remainingAmount < amount; // True if we added at least SOME
        }

        public bool RemoveItem(ItemData data, int amount)
        {
            if (data == null || amount <= 0 || GetItemCount(data) < amount) return false;

            int remainingAmount = amount;

            // Loop backwards so we deplete last slots first (common paradigm)
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                var slot = slots[i];
                if (!slot.IsEmpty && slot.Item.Data == data)
                {
                    if (slot.Item.CurrentStack >= remainingAmount)
                    {
                        slot.Item.RemoveFromStack(remainingAmount);
                        slot.RefreshSlot();

                        OnItemRemoved?.Invoke(data, amount);
                        OnInventoryUpdated?.Invoke();
                        return true;
                    }
                    else
                    {
                        remainingAmount -= slot.Item.CurrentStack;
                        slot.Item.RemoveFromStack(slot.Item.CurrentStack);
                        slot.RefreshSlot();
                    }
                }
            }

            OnInventoryUpdated?.Invoke();
            return true;
        }

        public bool HasItem(ItemData data)
        {
            return GetItemCount(data) > 0;
        }

        public int GetItemCount(ItemData data)
        {
            int count = 0;
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.Item.Data == data)
                {
                    count += slot.Item.CurrentStack;
                }
            }
            return count;
        }

        public void ClearInventory()
        {
            foreach (var slot in slots)
            {
                slot.ClearSlot();
            }
            OnInventoryUpdated?.Invoke();
        }

        public List<InventorySlot> GetAllSlots()
        {
            return slots;
        }

        // Additional: Direct manipulation (Drag & Drop)
        public bool SwapSlots(int slotIndex1, int slotIndex2)
        {
            if (slotIndex1 < 0 || slotIndex1 >= slots.Count || slotIndex2 < 0 || slotIndex2 >= slots.Count)
                return false;

            var item1 = slots[slotIndex1].Item;
            var item2 = slots[slotIndex2].Item;

            slots[slotIndex1].SetItem(item2);
            slots[slotIndex2].SetItem(item1);

            OnInventoryUpdated?.Invoke();
            return true;
        }
    }
}
