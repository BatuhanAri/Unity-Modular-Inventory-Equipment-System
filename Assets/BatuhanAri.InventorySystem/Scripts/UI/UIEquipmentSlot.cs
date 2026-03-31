using UnityEngine;
using UnityEngine.UI;
using BatuhanAri.InventorySystem.Equipment;

namespace BatuhanAri.InventorySystem.UI
{
    public class UIEquipmentSlot : UISlot
    {
        [Header("Equipment Specific")]
        [SerializeField] private Image placeholderIcon;

        public EquipmentSlot DataSlot { get; private set; }
        private UIInventoryWindow inventoryWindow;

        public void Initialize(EquipmentSlot slotData, UIInventoryWindow window)
        {
            this.DataSlot = slotData;
            this.inventoryWindow = window;

            DataSlot.OnEquipmentUpdated += OnEquipmentUpdated;
            RefreshSlot();
        }

        private void OnDestroy()
        {
            if (DataSlot != null)
            {
                DataSlot.OnEquipmentUpdated -= OnEquipmentUpdated;
            }
        }

        private void OnEquipmentUpdated(EquipmentSlot slot)
        {
            RefreshSlot();
        }

        public override void RefreshSlot()
        {
            this.ItemRef = DataSlot.CurrentItem;
            UpdateVisuals();
            
            if (placeholderIcon != null)
            {
                placeholderIcon.gameObject.SetActive(ItemRef == null);
            }
        }

        protected override void HandleDrop(UISlot droppedSlot)
        {
            if (droppedSlot is UIInventorySlot otherInvSlot)
            {
                // Try equip
                if (otherInvSlot.ItemRef != null)
                {
                    inventoryWindow.EquipmentManager.Equip(otherInvSlot.ItemRef, inventoryWindow.InventoryManager);
                }
            }
            // Swap between equipments is rare and usually handled differently (or ignored)
        }
    }
}

