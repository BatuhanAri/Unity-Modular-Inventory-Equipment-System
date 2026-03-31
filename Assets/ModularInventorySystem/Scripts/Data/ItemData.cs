using UnityEngine;
using ModularInventory.Core;

namespace ModularInventory.Data
{
    /// <summary>
    /// Base definitions for all items in the game.
    /// Create specific item types by creating assets from this class.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Modular Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic Information")]
        [Tooltip("Unique ID for saving and networking purposes.")]
        public string ID = System.Guid.NewGuid().ToString();
        public string Name = "New Item";
        [TextArea(3, 5)]
        public string Description;
        public Sprite Icon;

        [Header("Classification")]
        public ItemType ItemType;
        public EquipmentType EquipmentType;
        public ItemRarity Rarity;

        [Header("Stats & Properties")]
        public int MaxStack = 1;
        public float Weight = 0.5f;

        [Header("Visuals")]
        [Tooltip("Optional 3D Prefab for dropping this item into the world.")]
        public GameObject Prefab;

        private void OnValidate()
        {
            // Auto generate ID if empty in editor
            if (string.IsNullOrEmpty(ID))
            {
                ID = System.Guid.NewGuid().ToString();
            }

            // Ensure we don't drop MaxStack below 1.
            if (MaxStack < 1)
            {
                MaxStack = 1;
            }
        }
    }
}
