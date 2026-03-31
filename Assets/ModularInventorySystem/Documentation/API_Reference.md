# 📚 API Reference Guide

Welcome to the API reference for the **Modular Inventory & Equipment System**. This document outlines the primary classes, interfaces, and methods you can use to extend or integrate the system into your own game loops.

## Core Interfaces

### `IInventory`
Located in `ModularInventory.Core`. This interface dictates the underlying rules for ANY inventory implementation, meaning you can write your own managers if needed.
- `bool AddItem(ItemData data, int amount)`: Attempts to add an item. Returns true if fully added.
- `bool RemoveItem(ItemData data, int amount)`: Removes the specified amount. Fails and returns false if not enough items exist.
- `bool HasItem(ItemData data)`: Returns true if count > 0.
- `int GetItemCount(ItemData data)`: Returns total cumulative stack of the item.
- `void ClearInventory()`: Empties all slots instantly.
- `List<InventorySlot> GetAllSlots()`: Returns raw slots for UI or iterations.

## Managers

### `InventoryManager : MonoBehaviour, IInventory`
The core component managing storage logic and bounds.

#### Key Methods
- `void InitializeInventory(int size)`: Rebuilds the internal slot list. Useful if expanding backpack size dynamically at runtime.
- `bool SwapSlots(int slotIndex1, int slotIndex2)`: Used by UI to swap two items. Will attempt to **merge** them if they are identical stackable items.
- `bool SplitSlot(int sourceSlot, int targetSlot, int amount)`: Moves a specific amount of items from one slot to another (or empty) slot. Used primarily for "Half-Split" dragging.

#### Events
- `public event Action<ItemData, int> OnItemAdded;`
- `public event Action<ItemData, int> OnItemRemoved;`
- `public event Action OnInventoryUpdated;` (Fires immediately whenever a slot changes state for UI refresh).

---

### `EquipmentManager : MonoBehaviour`
Handles typed slots (Head, Chest, Weapon, etc.).

#### Key Methods
- `EquipmentSlot GetSlot(EquipmentType type)`: Returns the specific logic slot.
- `bool Equip(InventoryItem item, InventoryManager inventory)`: Safely pulls an item from standard inventory, swaps the current equipped item (if any) back into the standard inventory, and equips the new one. Preserves the item's `RuntimeID`.
- `bool Unequip(EquipmentType type, InventoryManager inventory)`: Reverses the equip process.

#### Events
- `public event Action<InventoryItem, EquipmentType> OnItemEquipped;`
- `public event Action<InventoryItem, EquipmentType> OnItemUnequipped;`

---

## Save System Abstraction
Located in `ModularInventory.SaveSystem`.

### `ISaveProvider`
To replace the default `PlayerPrefs` implementation, inherit this interface on a custom class.
- `void Save(string key, string jsonData);`
- `string Load(string key);`
- `bool HasSave(string key);`

**Example usage with EasySave 3:**
```csharp
public class EasySaveProvider : MonoBehaviour, ISaveProvider {
    public void Save(string k, string j) => ES3.Save(k, j);
    public string Load(string k) => ES3.LoadString(k, "");
    public bool HasSave(string k) => ES3.KeyExists(k);
}
```
