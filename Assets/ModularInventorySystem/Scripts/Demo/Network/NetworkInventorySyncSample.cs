using UnityEngine;
using ModularInventory.Data;
using ModularInventory.Inventory;

namespace ModularInventory.Demo.Network
{
    /// <summary>
    /// This is an architectural SAMPLE of how to wrap the Modular Inventory Manager
    /// for a typical Server-Authoritative multiplayer game (like Netcode for GameObjects or Photon Fusion).
    /// </summary>
    public class NetworkInventorySyncSample : MonoBehaviour
    {
        [Header("Local Components")]
        [SerializeField] private InventoryManager localInventory;
        
        // Pseudo-code for Netcode: 
        // public bool IsServer => ...;
        // public bool IsOwner => ...;

        private void OnEnable()
        {
            // Only the server should theoretically approve item additions,
            // but for UI responsiveness, you might add it locally first and 
            // rollback if the server denies.
            
            // localInventory.OnItemAdded += HandleItemAdded;
        }

        private void OnDisable()
        {
            // localInventory.OnItemAdded -= HandleItemAdded;
        }

        /* 
         * EXAMPLE: A player clicks to pick up an item.
         */
        public void RequestPickupItem(string itemID, int amount)
        {
            // Instead of directly calling inventoryManager.AddItem(data),
            // We tell the server to do it.

            // if (IsOwner) {
            //      ServerRpc_RequestPickup(itemID, amount);
            // }
        }

        /*
         * [ServerRpc]
         * private void ServerRpc_RequestPickup(string itemID, int amount)
         * {
         *     if (!IsServer) return;
         *     
         *     // Validate if item exists on ground near player...
         *     // Find ItemData from Database...
         *     
         *     bool success = localInventory.AddItem(itemData, amount);
         *     if (success) {
         *          // Broadcast to all clients (or just the owner) to update their local UI memory
         *          ClientRpc_SyncInventoryAddition(itemID, amount);
         *     }
         * }
         */

         /*
         * [ClientRpc]
         * private void ClientRpc_SyncInventoryAddition(string itemID, int amount)
         * {
         *     if (IsServer) return; // Server already added it.
         *     
         *     // Find ItemData...
         *     // Force UI update locally bypassing validation:
         *     localInventory.AddItem(itemData, amount); 
         * }
         */
    }
}
