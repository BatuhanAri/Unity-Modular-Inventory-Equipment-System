using System.Collections.Generic;
using UnityEngine;
using ModularInventory.Inventory;
using ModularInventory.Data;
using System.Linq;

namespace ModularInventory.SaveSystem
{
    [System.Serializable]
    public class InventorySaveData
    {
        public List<SlotSaveData> Slots = new List<SlotSaveData>();
    }

    [System.Serializable]
    public class SlotSaveData
    {
        public int SlotIndex;
        public string ItemID;
        public int StackAmount;
    }

    public class InventorySaveSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryManager inventoryManager;
        
        [Header("Settings")]
        [SerializeField] private string saveKey = "PlayerInventorySave";
        
        // Caching items for loading based on ID
        private Dictionary<string, ItemData> itemDatabase;

        private void Awake()
        {
            // Simple way to load all items (Place ItemData ScriptableObjects in Resources/Items)
            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            itemDatabase = allItems.ToDictionary(item => item.ID, item => item);
        }

        public void SaveInventory()
        {
            if (inventoryManager == null) return;

            InventorySaveData saveData = new InventorySaveData();

            foreach (var slot in inventoryManager.GetAllSlots())
            {
                if (!slot.IsEmpty)
                {
                    saveData.Slots.Add(new SlotSaveData() {
                        SlotIndex = slot.SlotIndex,
                        ItemID = slot.Item.Data.ID,
                        StackAmount = slot.Item.CurrentStack
                    });
                }
            }

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(saveKey, json);
            PlayerPrefs.Save();
            
            Debug.Log("Inventory Saved: " + json);
        }

        public void LoadInventory()
        {
            if (inventoryManager == null) return;

            if (!PlayerPrefs.HasKey(saveKey))
            {
                Debug.LogWarning("No save data found");
                return;
            }

            inventoryManager.ClearInventory();
            string json = PlayerPrefs.GetString(saveKey);
            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

            foreach (var slotData in saveData.Slots)
            {
                if (itemDatabase.TryGetValue(slotData.ItemID, out ItemData parsedData))
                {
                    InventoryItem newItem = new InventoryItem(parsedData, slotData.StackAmount);
                    // Force inject into the correct slot
                    var matchingSlot = inventoryManager.GetAllSlots().Find(s => s.SlotIndex == slotData.SlotIndex);
                    if (matchingSlot != null)
                    {
                        matchingSlot.SetItem(newItem);
                    }
                }
            }
            
            Debug.Log("Inventory Loaded.");
        }
    }
}
