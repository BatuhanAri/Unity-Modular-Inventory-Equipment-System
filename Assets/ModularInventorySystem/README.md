# 📦 Modular Inventory & Equipment System

**The Ultimate Plug & Play Inventory Solution for Unity 2022 LTS+**

Welcome to the **Modular Inventory & Equipment System**! Designed with SOLID principles and Event-Driven architecture, this asset provides a robust, fully extensible, and highly optimized inventory foundation for your next big hit.

Whether you're building a simple RPG, a complex survival game, or an MMO, our system gives you the core tools needed without boxing you into rigid code constraints.

---

## 🌟 Key Features

*   **100% Modularity:** Designed with Interfaces (`IInventory`) and loose coupling. Easy to integrate your own custom logic.
*   **ScriptableObject Driven:** Define your items (Consumables, Materials, Equipments) effortlessly from the Inspector.
*   **Drag & Drop UI:** Built-in mobile-friendly `Canvas` UI with intuitive drag & drop, and stack splitting capabilities.
*   **Equipment Manager:** Equip items to specific slots (Head, Chest, Weapon, etc.) with automatic stat management routing and rule validation.
*   **Zero-GC Allocations (Optimized):** Carefully written to be mobile and VR friendly. Built for 500+ items at 60fps.
*   **Integrated Save/Load System:** JSON-based persistent saving works out of the box with `PlayerPrefs`.
*   **Smart Tooltip:** Out-of-the-box UI tooltips that respond instantly when hovering items.

---

## 🚀 5-Minute Quick Setup

1. **Import the Package:** Drag the `.unitypackage` into your project (or if dragging the folder, make sure all scripts compile automatically thanks to our `.asmdef`).
2. **Setup your Items:** Right click in your Project Window -> `Create -> Modular Inventory -> Item Data`. Define your item's name, icon, and max stack depth.
3. **Drop the Prefab:** Drag the `System_Inventory` and `UI_Canvas` prefabs (found in the `Prefabs` folder) into your main scene.
4. **Connect UI (Optional):** If using the provided UI, make sure the `UIInventoryWindow` references the `InventoryManager` component!
5. **Add Items via Script:**
    ```csharp
    // Add 5 health potions dynamically
    inventoryManager.AddItem(healthPotionData, 5);
    ```

---

## 📖 API High-Level Overview

*   `InventoryManager`: Implements `IInventory`. Call `.AddItem(data, amount)`, `.RemoveItem(data, amount)`.
*   `EquipmentManager`: Call `.Equip(item)` from UI or scripts. Fires `OnItemEquipped` and `OnItemUnequipped` events.
*   `InventorySaveSystem`: Use `SaveInventory()` to serialize the current `InventoryManager` state to JSON and `LoadInventory()` to restore it.
   
*Note: Make sure your `ItemData` ScriptableObjects are located in a folder named `Resources/Items` for the SaveSystem to automatically map IDs effectively.*

---

## 🛠 Advanced Developer Guide (Extending the system)

If you're creating a Multiplayer game (e.g., using Netcode for GameObjects or Photon Fusion), you only need to sync the `RuntimeID` or the `SlotIndex`. Since the core logic relies on abstract interfaces (`IInventory`), you can easily write a `NetworkInventoryManager` wrapper that intercepts `AddItem` before calling the base function!

If you want to modify UI behavior, inherit from `UISlot` and implement your custom `HandleDrop` interactions.

---

### Support & Documentation
Found an issue or want to request a feature? Contact us through our publisher page on the Unity Asset Store and don't forget to **leave a 5-star review** if this helped speed up your development timeframe! 

---
*Created with cleanly separated namespaces, minimal LINQ usage, and maximum performance focus.*
