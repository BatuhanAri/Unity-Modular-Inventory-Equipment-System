using System.Collections.Generic;
using UnityEngine;
using ModularInventory.Inventory;
using ModularInventory.Equipment;

namespace ModularInventory.UI
{
    public class UIInventoryWindow : MonoBehaviour
    {
        [Header("Managers")]
        public InventoryManager InventoryManager;
        public EquipmentManager EquipmentManager;

        [Header("UI Grids & Layouts")]
        [SerializeField] private RectTransform inventorySlotParent;
        [SerializeField] private RectTransform equipmentSlotParent;

        [Header("Prefabs")]
        [SerializeField] private UIInventorySlot inventorySlotPrefab;
        [SerializeField] private UIEquipmentSlot equipmentSlotPrefab;

        private List<UIInventorySlot> generatedInventorySlots = new List<UIInventorySlot>();
        private List<UIEquipmentSlot> generatedEquipmentSlots = new List<UIEquipmentSlot>();

        private void Start()
        {
            if (InventoryManager != null)
            {
                GenerateInventoryUI();
            }

            if (EquipmentManager != null)
            {
                GenerateEquipmentUI();
            }
        }

        private void GenerateInventoryUI()
        {
            // Clear existing
            foreach (var child in generatedInventorySlots)
            {
                Destroy(child.gameObject);
            }
            generatedInventorySlots.Clear();

            // Instantiate
            foreach (var slotData in InventoryManager.GetAllSlots())
            {
                UIInventorySlot slotUI = Instantiate(inventorySlotPrefab, inventorySlotParent);
                slotUI.Initialize(slotData, this);
                generatedInventorySlots.Add(slotUI);
            }
        }

        private void GenerateEquipmentUI()
        {
            foreach (var child in generatedEquipmentSlots)
            {
                Destroy(child.gameObject);
            }
            generatedEquipmentSlots.Clear();

            foreach (var slotData in EquipmentManager.GetAllSlots())
            {
                UIEquipmentSlot slotUI = Instantiate(equipmentSlotPrefab, equipmentSlotParent);
                // Assign slot specific name or position logic here if needed
                slotUI.Initialize(slotData, this);
                generatedEquipmentSlots.Add(slotUI);
            }
        }
        
        // Use this to display/hide based on input (e.g., 'I' key)
        public void ToggleWindow()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
