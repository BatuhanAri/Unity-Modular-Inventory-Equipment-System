# Installation & Setup Guide

This guide will help you get the Modular Inventory & Equipment System up and running in your Unity project in under 5 minutes.

---

## Prerequisites

- **Unity Version:** 2022.3 LTS or higher
- **TextMeshPro:** Must be installed (comes by default with newer Unity versions)
- **Scripting Backend:** .NET 4.x or IL2CPP (both supported)

---

## Installation Steps

### Method 1: Using the Editor Menu (Recommended)

1. **Import the Package**
   - Download and import the asset into your Unity project
   - All files will be placed in `Assets/BatuhanAri.InventorySystem/`

2. **Quick Setup via Menu**
   - Go to `BatuhanAri → Inventory System → Setup Complete Inventory System`
   - This automatically creates:
     - InventoryManager (with 20 slots)
     - EquipmentManager
     - UI Canvas with inventory window

3. **That's it!** The system is ready to use.

### Method 2: Manual Setup

If you prefer manual control:

1. **Create InventoryManager**
   ```csharp
   var go = new GameObject("InventoryManager");
   var manager = go.AddComponent<InventoryManager>();
   manager.InitializeInventory(20); // 20 inventory slots
   ```

2. **Create EquipmentManager**
   ```csharp
   var equipGo = new GameObject("EquipmentManager");
   var equipManager = equipGo.AddComponent<EquipmentManager>();
   ```

3. **Create Sample Items**
   - Right-click in Project → `Create → BatuhanAri → Item Data`
   - Configure name, icon, type, and max stack
   - Save in `Resources/Items/` folder

4. **Setup UI (Optional)**
   - Create a Canvas in your scene
   - Add the `UIInventoryWindow` component
   - Reference your InventoryManager

---

## Creating Items

### Option 1: Via Editor Menu (Easiest)

1. Go to `BatuhanAri → Inventory System → Create Sample ItemData`
2. Item automatically created in `Assets/Resources/Items/HealthPotion.asset`
3. Edit properties in the Inspector

### Option 2: Manual Creation

1. Right-click in Project Window → `Create → BatuhanAri → Item Data`
2. Name it (e.g., `SwordOfFire.asset`)
3. In the Inspector, configure:
   - **Name:** Display name in-game
   - **Description:** Item tooltip text
   - **Item Type:** Consumable, Equipment, Material, etc.
   - **Equipment Type:** (if Equipment) Weapon, Armor, Helmet, etc.
   - **Max Stack:** How many can stack in one slot
   - **Rarity:** Common, Uncommon, Rare, Legendary, etc.
   - **Icon:** Sprite for UI display

---

## Basic Usage Examples

### Adding Items Programmatically

```csharp
using BatuhanAri.InventorySystem.Inventory;
using BatuhanAri.InventorySystem.Data;

public class GameManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemData healthPotionData;

    void Start()
    {
        // Add 5 health potions to inventory
        bool success = inventoryManager.AddItem(healthPotionData, 5);
        if (success)
            Debug.Log("Added 5 health potions!");
    }
}
```

### Listening to Inventory Events

```csharp
void Start()
{
    inventoryManager.OnItemAdded += HandleItemAdded;
    inventoryManager.OnItemRemoved += HandleItemRemoved;
}

void HandleItemAdded(ItemData item, int amount)
{
    Debug.Log($"Added {amount}x {item.Name}");
}

void HandleItemRemoved(ItemData item, int amount)
{
    Debug.Log($"Removed {amount}x {item.Name}");
}
```

### Equipping Items

```csharp
var equipmentManager = GetComponent<EquipmentManager>();
var inventoryItem = inventoryManager.GetAllSlots()[0].Item;

bool equipped = equipmentManager.Equip(inventoryItem, inventoryManager);
if (equipped)
    Debug.Log("Item equipped!");
```

### Saving and Loading

```csharp
using BatuhanAri.InventorySystem.SaveSystem;

var saveSystem = new InventorySaveSystem(inventoryManager);

// Save to PlayerPrefs
saveSystem.SaveInventory();

// Load from PlayerPrefs
saveSystem.LoadInventory();
```

---

## Folder Structure

```
Assets/
└── BatuhanAri.InventorySystem/
    ├── Scripts/
    │   ├── Core/                    # Interfaces and enums
    │   ├── Data/                    # ItemData and related classes
    │   ├── Inventory/               # InventoryManager and slot logic
    │   ├── Equipment/               # EquipmentManager
    │   ├── UI/                      # UI components
    │   ├── SaveSystem/              # Save/load functionality
    │   ├── Demo/                    # Example usage scripts
    │   └── Editor/                  # Editor tools (setup menu)
    ├── Tests/
    │   └── Editor/
    │       └── InventoryLogicTests.cs
    ├── Documentation/
    │   ├── API_Reference.md
    │   ├── Multiplayer_Setup.md
    │   ├── PLATFORM_COMPATIBILITY.md
    │   └── SETUP.md
    ├── Resources/
    │   └── Items/                   # Place ItemData assets here
    └── README.md
```

---

## Common Issues & Solutions

### Issue: "InventoryManager not found"
**Solution:** 
- Ensure you've called `InventoryManager.InitializeInventory(slotCount)` in Start() or Awake()
- Check that the component is attached to a GameObject in the scene

### Issue: "Items won't save"
**Solution:**
- Place ItemData ScriptableObjects in `Resources/Items/` folder
- Use `InventorySaveSystem.SaveInventory()` to explicitly save
- Check that ISaveProvider is properly configured

### Issue: "UI doesn't show items"
**Solution:**
- Verify `UIInventoryWindow.InventoryManager` is assigned
- Check that Canvas is set to `ScreenSpaceOverlay` render mode
- Ensure item icons are assigned in ItemData

### Issue: "Tests fail to run"
**Solution:**
- Go to `Window → Testing → Test Runner`
- Select "EditMode" tab
- Click "Run All" on the test assembly
- Check console for detailed error messages

---

## Next Steps

1. **Read the API Reference** - Understand all available methods and properties
2. **Review Multiplayer Guide** - If building networked games
3. **Check Platform Compatibility** - Ensure your target platform is supported
4. **Run Unit Tests** - Verify everything works: `BatuhanAri → Inventory System → Run Tests`
5. **Customize UI** - Modify `UIInventoryWindow` to match your game's style

---

## Performance Tips

- **Mobile:** Cap inventory to 300-400 items for smooth performance
- **WebGL:** Limit to 200 items to avoid serialization delays
- **Save System:** Use cloud-based providers for large inventories
- **UI:** Disable UI updates when inventory window is closed

---

## Support & Troubleshooting

For additional help:
1. Review the **[API Reference](API_Reference.md)**
2. Check the **[Platform Compatibility](PLATFORM_COMPATIBILITY.md)** guide
3. Review test cases in `Tests/Editor/InventoryLogicTests.cs` for working examples
4. Check console for error messages

---

## Updating from Previous Versions

If updating from v0.x to v1.0.0:
1. Backup your project
2. Remove old version completely
3. Import new version fresh
4. Re-apply any custom modifications
5. Run tests to verify compatibility

---

**Happy inventorying! 🎮**
