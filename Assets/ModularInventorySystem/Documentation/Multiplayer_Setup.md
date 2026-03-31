# 🌐 Multiplayer Setup Guide

The **Modular Inventory & Equipment System** separates UI from Core logic using the Observer Pattern, making it highly adaptable for Multiplayer engines like Unity Netcode for GameObjects (NGO), Mirror, or Photon Fusion.

## The Strategy

Instead of building heavy network libraries directly into the inventory system (which limits flexibility and bloats the asset), this package provides the raw structure needed to sync state across clients.

A typical server-authoritative multiplayer workflow requires intercepting the "Player Input" before it hits the `InventoryManager`.

### Step 1: Intercept Input
When a player clicks "Pick Up Item" or tries to Drag-and-Drop an item, **do not** call `InventoryManager.AddItem()` or `SwapSlots()` locally.

Instead, route that request to a Server RPC.
```csharp
// Example Netcode Client Request
[ServerRpc]
public void RequestPickupItemServerRpc(string itemID, int amount) {
    if (ServerValidatePickup(itemID)) {
        // Find ItemData from ID
        playerInventory.AddItem(itemData, amount);
        
        // Broadcast the change to clients (Optional if player state is synced automatically)
        SyncInventoryStateClientRpc(itemID, amount);
    }
}
```

### Step 2: Utilize the RuntimeID
If a player drops an enchanted sword or modified equipment, the raw `ItemData` ScriptableObject doesn't store the modifications; the `InventoryItem.RuntimeID` does.

Ensure that when syncing complex items over the network, you transmit both the ScriptableObject ID and the associated `RuntimeID` so all clients represent the exact same weapon instance!

### Check the Reference
Refer to `Scripts/Demo/Network/NetworkInventorySyncSample.cs` for a pseudo-code implementation showing Server/Client segregation.
