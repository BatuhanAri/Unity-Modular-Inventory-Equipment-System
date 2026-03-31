using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ModularInventory.Inventory;

namespace ModularInventory.UI
{
    /// <summary>
    /// Base class for both Inventory and Equipment slots in UI.
    /// Handles drag and drop interfaces.
    /// </summary>
    public abstract class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [Header("Base UI References")]
        [SerializeField] protected Image iconImage;
        [SerializeField] protected GameObject highlightIndicator;

        public InventoryItem ItemRef { get; protected set; }

        public abstract void RefreshSlot();
        protected abstract void HandleDrop(UISlot droppedSlot);

        protected virtual void UpdateVisuals()
        {
            if (ItemRef != null && ItemRef.CurrentStack > 0)
            {
                iconImage.sprite = ItemRef.Data.Icon;
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.color = Color.clear;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (highlightIndicator != null) highlightIndicator.SetActive(true);
            if (ItemRef != null)
            {
                UITooltip.Instance?.ShowTooltip(ItemRef.Data);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (highlightIndicator != null) highlightIndicator.SetActive(false);
            UITooltip.Instance?.HideTooltip();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ItemRef == null) return;
            
            // Partially fade original icon
            Color c = iconImage.color;
            c.a = 0.5f;
            iconImage.color = c;

            UIDragDropManager.Instance?.StartDragging(this);
            UITooltip.Instance?.HideTooltip();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Handled by UIDragDropManager globally mapping position
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            UIDragDropManager.Instance?.StopDragging();
            UpdateVisuals(); // Restore full alpha if still here
        }

        public void OnDrop(PointerEventData eventData)
        {
            UISlot draggedSlot = UIDragDropManager.Instance?.DraggedSlot;
            if (draggedSlot != null && draggedSlot != this)
            {
                HandleDrop(draggedSlot);
            }
        }
    }
}
