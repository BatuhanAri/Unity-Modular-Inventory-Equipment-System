using UnityEngine;
using UnityEngine.UI;
using ModularInventory.Inventory;

namespace ModularInventory.UI
{
    /// <summary>
    /// Singleton manager to handle the currently dragged item icon across the entire Canvas.
    /// </summary>
    public class UIDragDropManager : MonoBehaviour
    {
        public static UIDragDropManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private RectTransform dragIconTransform;
        [SerializeField] private Image dragIconImage;

        public UISlot DraggedSlot { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                HideDragIcon();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartDragging(UISlot originSlot)
        {
            if (originSlot == null || originSlot.ItemRef == null) return;
            
            DraggedSlot = originSlot;
            
            dragIconImage.sprite = originSlot.ItemRef.Data.Icon;
            dragIconImage.gameObject.SetActive(true);
            UpdateDragPosition();
        }

        public void StopDragging()
        {
            DraggedSlot = null;
            HideDragIcon();
        }

        private void HideDragIcon()
        {
            if (dragIconImage != null)
                dragIconImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (DraggedSlot != null)
            {
                UpdateDragPosition();
            }
        }

        private void UpdateDragPosition()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)dragIconTransform.parent,
                Input.mousePosition,
                null,
                out position
            );
            dragIconTransform.anchoredPosition = position;
        }
    }
}
