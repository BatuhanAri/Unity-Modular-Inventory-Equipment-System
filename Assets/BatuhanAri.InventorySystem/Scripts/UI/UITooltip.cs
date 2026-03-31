using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BatuhanAri.InventorySystem.Data;

namespace BatuhanAri.InventorySystem.UI
{
    /// <summary>
    /// Global tooltip window to display item properties when hovered.
    /// </summary>
    public class UITooltip : MonoBehaviour
    {
        public static UITooltip Instance { get; private set; }

        [Header("UI Element References")]
        [SerializeField] private GameObject tooltipBox;
        [SerializeField] private RectTransform tooltipRectTransform;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI statsText;

        [Header("Positioning Settings")]
        [SerializeField] private Vector2 offset = new Vector2(10, -10);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                HideTooltip();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (tooltipBox != null && tooltipBox.activeSelf)
            {
                FollowMouse();
            }
        }

        public void ShowTooltip(ItemData itemData)
        {
            if (itemData == null) return;

            titleText.text = itemData.Name;
            typeText.text = $"{itemData.Rarity.ToString()} {itemData.ItemType.ToString()}";
            descriptionText.text = itemData.Description;
            
            // Build stats based on what type of item it is
            string stats = string.Empty;
            if (itemData.ItemType == Core.ItemType.Equipment)
            {
                stats += $"Slot: {itemData.EquipmentType}\n";
            }
            stats += $"Weight: {itemData.Weight}kg";
            
            statsText.text = stats;

            tooltipBox.SetActive(true);
            FollowMouse(); // Update position immediately
        }

        public void HideTooltip()
        {
            if (tooltipBox != null) tooltipBox.SetActive(false);
        }

        private void FollowMouse()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)tooltipRectTransform.parent,
                Input.mousePosition,
                null,
                out position
            );
            tooltipRectTransform.anchoredPosition = position + offset;
        }
    }
}

