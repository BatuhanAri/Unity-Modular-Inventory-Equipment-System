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
        
        private ISaveProvider saveProvider;
        private Dictionary<string, ItemData> itemDatabase;

        private void Awake()
        {
            // Try to find a Save Provider attached to the same object or globally
            saveProvider = GetComponent<ISaveProvider>();
            if (saveProvider == null)
            {
                Debug.LogWarning("No ISaveProvider found on InventorySaveSystem GameObject. Please attach one (e.g. PlayerPrefsSaveProvider).");
            }

            ItemData[] allItems = Resources.LoadAll<ItemData>("Items");
            itemDatabase = allItems.ToDictionary(item => item.ID, item => item);
        }

        public void SaveInventory()
        {
            if (inventoryManager == null || saveProvider == null) return;

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
            saveProvider.Save(saveKey, json);
            
            Debug.Log("Inventory Saved using " + saveProvider.GetType().Name);
        }

        public void LoadInventory()
        {
            if (inventoryManager == null || saveProvider == null) return;

            if (!saveProvider.HasSave(saveKey))
            {
                Debug.LogWarning("No save data found");
                return;
            }

            inventoryManager.ClearInventory();
            string json = saveProvider.Load(saveKey);
            
            if (string.IsNullOrEmpty(json)) return;

            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

            foreach (var slotData in saveData.Slots)
            {
                if (itemDatabase.TryGetValue(slotData.ItemID, out ItemData parsedData))
                {
                    InventoryItem newItem = new InventoryItem(parsedData, slotData.StackAmount);
                    var matchingSlot = inventoryManager.GetAllSlots().Find(s => s.SlotIndex == slotData.SlotIndex);
                    if (matchingSlot != null)
                    {
                        matchingSlot.SetItem(newItem);
                    }
                }
            }
            
            Debug.Log("Inventory Loaded via " + saveProvider.GetType().Name);
        }
    }
}
