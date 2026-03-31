using TMPro;
using UnityEngine;
using BatuhanAri.InventorySystem.Inventory;

namespace BatuhanAri.InventorySystem.UI
{
    public class UIInventorySlot : UISlot
    {
        [Header("Inventory Specific")]
        [SerializeField] private TextMeshProUGUI stackText;

        private InventorySlot dataSlot;
        private UIInventoryWindow inventoryWindow;

        public void Initialize(InventorySlot slotData, UIInventoryWindow window)
        {
            this.dataSlot = slotData;
            this.inventoryWindow = window;
            
            dataSlot.OnSlotUpdated += OnDataSlotUpdated;
            RefreshSlot();
        }

        private void OnDestroy()
        {
            if (dataSlot != null)
            {
                dataSlot.OnSlotUpdated -= OnDataSlotUpdated;
            }
        }

        private void OnDataSlotUpdated(InventorySlot slot)
        {
            RefreshSlot();
        }

        public override void RefreshSlot()
        {
            this.ItemRef = dataSlot.Item;
            UpdateVisuals();
            UpdateStackText();
        }

        private void UpdateStackText()
        {
            if (stackText == null) return;
            
            if (ItemRef != null && ItemRef.CurrentStack > 1)
            {
                stackText.text = ItemRef.CurrentStack.ToString();
                stackText.gameObject.SetActive(true);
            }
            else
            {
                stackText.gameObject.SetActive(false);
            }
        }

        protected override void HandleDrop(UISlot droppedSlot)
        {
            if (droppedSlot is UIInventorySlot otherInvSlot)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    // Half-split logic
                    if (otherInvSlot.ItemRef != null && otherInvSlot.ItemRef.CurrentStack > 1)
                    {
                        int halfAmount = otherInvSlot.ItemRef.CurrentStack / 2;
                        inventoryWindow.InventoryManager.SplitSlot(otherInvSlot.dataSlot.SlotIndex, this.dataSlot.SlotIndex, halfAmount);
                    }
                    else
                    {
                        // Fallback to swap if item is 1 stack
                        inventoryWindow.InventoryManager.SwapSlots(otherInvSlot.dataSlot.SlotIndex, this.dataSlot.SlotIndex);
                    }
                }
                else
                {
                    // Normal Swap (or Merge if they are same item type, which is handled in SwapSlots now)
                    inventoryWindow.InventoryManager.SwapSlots(otherInvSlot.dataSlot.SlotIndex, this.dataSlot.SlotIndex);
                }
            }
            else if (droppedSlot is UIEquipmentSlot equipSlot)
            {
                // Unequip attempt manually
                inventoryWindow.EquipmentManager.Unequip(equipSlot.DataSlot.AllowedType, inventoryWindow.InventoryManager);
            }
        }
    }
}

