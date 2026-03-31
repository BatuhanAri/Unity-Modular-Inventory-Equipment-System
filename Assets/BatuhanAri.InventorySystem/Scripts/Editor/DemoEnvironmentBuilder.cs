using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BatuhanAri.InventorySystem.Core;
using BatuhanAri.InventorySystem.Inventory;
using BatuhanAri.InventorySystem.Equipment;
using BatuhanAri.InventorySystem.UI;
using BatuhanAri.InventorySystem.Data;
using System.IO;

namespace BatuhanAri.InventorySystem.Editor
{
    public class DemoEnvironmentBuilder : UnityEditor.EditorWindow
    {
        [MenuItem("BatuhanAri/Generate Playable Demo Scene")]
        public static void BuildDemoScene()
        {
            if (EditorUtility.DisplayDialog("Generate Demo", "This will create a new Canvas, EventSystem, and Inventory Managers in your current active scene. Proceed?", "Yes", "Cancel"))
            {
                CreateDemoEnvironment();
            }
        }

        private static void CreateDemoEnvironment()
        {
            // 1. Create Core Managers
            GameObject managersGo = new GameObject("--- INVENTORY SYSTEM MANAGERS ---");
            var invManager = managersGo.AddComponent<InventoryManager>();
            var equipManager = managersGo.AddComponent<EquipmentManager>();
            invManager.InitializeInventory(20);

            // 2. Create Event System if missing
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject esGo = new GameObject("EventSystem");
                esGo.AddComponent<EventSystem>();
                esGo.AddComponent<StandaloneInputModule>();
            }

            // 3. Create Canvas
            GameObject canvasGo = new GameObject("Inventory Canvas");
            Canvas canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.AddComponent<GraphicRaycaster>();

            // 4. Create UI Window
            GameObject windowGo = new GameObject("UI_InventoryWindow");
            windowGo.transform.SetParent(canvasGo.transform, false);
            var rect = windowGo.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(600, 400);
            windowGo.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f, 0.9f); // Dark background

            var uiWindow = windowGo.AddComponent<UIInventoryWindow>();
            uiWindow.InventoryManager = invManager;
            uiWindow.EquipmentManager = equipManager;

            // 5. Setup Grids for Slots
            GameObject invGrid = new GameObject("Inventory_Grid");
            invGrid.transform.SetParent(windowGo.transform, false);
            var gridRect = invGrid.AddComponent<RectTransform>();
            gridRect.anchorMin = new Vector2(0, 0);
            gridRect.anchorMax = new Vector2(0.5f, 1);
            gridRect.offsetMin = new Vector2(10, 10);
            gridRect.offsetMax = new Vector2(-10, -10);
            var glg = invGrid.AddComponent<GridLayoutGroup>();
            glg.cellSize = new Vector2(60, 60);
            glg.spacing = new Vector2(10, 10);

            // Create sample prefabs if they don't exist
            // (Normally you'd load these from Resources or GUID, but creating dynamically guarantees 100% out-of-the-box working)
            
            // 6. Generate Demo Item
            string itemPath = "Assets/BatuhanAri.InventorySystem/Demo/EpicSwordData.asset";
            ItemData swordItem = AssetDatabase.LoadAssetAtPath<ItemData>(itemPath);
            if (swordItem == null)
            {
                swordItem = CreateInstance<ItemData>();
                swordItem.Name = "Sword of the AI";
                swordItem.ItemType = ItemType.Equipment;
                swordItem.EquipmentType = EquipmentType.Weapon;
                swordItem.Rarity = ItemRarity.Epic;
                swordItem.MaxStack = 1;
                
                // Attempt to load the generated icon
                Sprite icon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/BatuhanAri.InventorySystem/Demo/Art/epic_sword_icon.png");
                if (icon != null) swordItem.Icon = icon;

                if (!Directory.Exists("Assets/BatuhanAri.InventorySystem/Demo")) Directory.CreateDirectory("Assets/BatuhanAri.InventorySystem/Demo");
                AssetDatabase.CreateAsset(swordItem, itemPath);
                AssetDatabase.SaveAssets();
            }

            // 7. Inject Items into Inventory for immediate testing
            invManager.AddItem(swordItem, 1);

            // 8. Tooltip System
            GameObject tooltipGo = new GameObject("UI_Tooltip");
            tooltipGo.transform.SetParent(canvasGo.transform, false);
            tooltipGo.SetActive(false); // Initially hidden
            // Adding component
            tooltipGo.AddComponent<UITooltip>();

            // 9. UIDragDropManager
            GameObject dragDropGo = new GameObject("UI_DragDropManager");
            dragDropGo.transform.SetParent(canvasGo.transform, false);
            dragDropGo.AddComponent<UIDragDropManager>();

            Debug.Log("Demo environment built successfully! Press Play to test.");
        }
    }
}
