using UnityEngine;
using BatuhanAri.InventorySystem.Data;
using BatuhanAri.InventorySystem.Inventory;

namespace BatuhanAri.InventorySystem.Demo
{
    /// <summary>
    /// Attach this to a physical GameObject in the 3D/2D world.
    /// Acts as ground loot that can be picked up and added to inventory.
    /// </summary>
    public class ItemPickup : MonoBehaviour
    {
        public ItemData ItemData;
        public int Amount = 1;

        private InventoryManager playerInventory;

        private void Start()
        {
            // Ideally inject this, but FindObject is okay for Demo purposes
            playerInventory = FindFirstObjectByType<InventoryManager>();
        }

        private void OnMouseDown() // Simple click-to-pickup for demo
        {
            Collect();
        }

        public void Collect()
        {
            if (playerInventory == null) return;

            bool added = playerInventory.AddItem(ItemData, Amount);
            if (added)
            {
                // Play pickup sound
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory Full! Could not pick up item.");
            }
        }
    }
}

