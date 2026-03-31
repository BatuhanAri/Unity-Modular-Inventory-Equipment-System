using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace BatuhanAri.InventorySystem.Editor
{
    /// <summary>
    /// Menu utility for quickly setting up the Inventory System in a scene.
    /// Provides shortcuts for creating and configuring required components.
    /// </summary>
    public class InventorySystemSetup
    {
        private const string MenuRoot = "BatuhanAri/Inventory System/";
        private const int InventorySlots = 20;
        private const int InitialGold = 100;

        /// <summary>
        /// Creates a fully configured Inventory System in the current scene.
        /// Includes InventoryManager and EquipmentManager components.
        /// </summary>
        [MenuItem(MenuRoot + "Setup Complete Inventory System")]
        public static void SetupCompleteSystem()
        {
            // Create main system GameObject
            var systemGO = new GameObject("InventorySystem");
            systemGO.tag = "Manager";

            // Add InventoryManager
            var inventoryManager = systemGO.AddComponent<InventoryManager>();
            inventoryManager.InitializeInventory(InventorySlots);

            // Add EquipmentManager
            var equipmentManager = systemGO.AddComponent<EquipmentManager>();

            // Set as DontDestroyOnLoad for persistence across scenes (optional)
            // Object.DontDestroyOnLoad(systemGO);

            // Create UI Canvas if it doesn't exist
            if (FindObjectOfType<Canvas>() == null)
            {
                SetupInventoryUI(inventoryManager);
            }

            // Mark scene as dirty
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log($"✅ Inventory System setup complete! Created {InventorySlots} slots.");
            Selection.activeGameObject = systemGO;
        }

        /// <summary>
        /// Creates only the InventoryManager component.
        /// </summary>
        [MenuItem(MenuRoot + "Create InventoryManager Only")]
        public static void CreateInventoryManagerOnly()
        {
            var go = new GameObject("InventoryManager");
            var manager = go.AddComponent<InventoryManager>();
            manager.InitializeInventory(InventorySlots);

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("✅ InventoryManager created and initialized.");
            Selection.activeGameObject = go;
        }

        /// <summary>
        /// Creates only the EquipmentManager component.
        /// </summary>
        [MenuItem(MenuRoot + "Create EquipmentManager Only")]
        public static void CreateEquipmentManagerOnly()
        {
            var go = new GameObject("EquipmentManager");
            var manager = go.AddComponent<EquipmentManager>();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Debug.Log("✅ EquipmentManager created.");
            Selection.activeGameObject = go;
        }

        /// <summary>
        /// Creates the UI Canvas for inventory display.
        /// </summary>
        [MenuItem(MenuRoot + "Create Inventory UI Canvas")]
        public static void CreateInventoryUICanvas()
        {
            var inventoryManager = FindObjectOfType<InventoryManager>();

            if (inventoryManager == null)
            {
                EditorUtility.DisplayDialog("Error", "Please create an InventoryManager first!", "OK");
                return;
            }

            SetupInventoryUI(inventoryManager);
            Debug.Log("✅ Inventory UI Canvas created and configured.");
        }

        /// <summary>
        /// Creates a sample item for testing.
        /// </summary>
        [MenuItem(MenuRoot + "Create Sample ItemData")]
        public static void CreateSampleItem()
        {
            string folderPath = "Assets/Resources";

            // Create Resources folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string itemsPath = $"{folderPath}/Items";
            if (!AssetDatabase.IsValidFolder(itemsPath))
            {
                AssetDatabase.CreateFolder(folderPath, "Items");
            }

            // Create sample ItemData
            var itemData = ScriptableObject.CreateInstance<ItemData>();
            itemData.Name = "Health Potion";
            itemData.Description = "Restores 50 health points";
            itemData.ItemType = ItemType.Consumable;
            itemData.MaxStack = 10;
            itemData.Rarity = ItemRarity.Common;

            string path = $"{itemsPath}/HealthPotion.asset";
            AssetDatabase.CreateAsset(itemData, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"✅ Sample ItemData created at: {path}");
            EditorGUIUtility.PingAsset(itemData);
        }

        /// <summary>
        /// Opens the test runner for running the inventory tests.
        /// </summary>
        [MenuItem(MenuRoot + "Run Tests")]
        public static void OpenTestRunner()
        {
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.TestTools.TestRunner.TestRunnerWindow,UnityEditor.TestTools.TestRunner"));
            Debug.Log("📊 Test Runner opened. Navigate to 'Editor' tests to run InventoryLogicTests.");
        }

        /// <summary>
        /// Opens the API reference documentation.
        /// </summary>
        [MenuItem(MenuRoot + "Documentation/API Reference")]
        public static void OpenAPIReference()
        {
            string docPath = "Assets/BatuhanAri.InventorySystem/Documentation/API_Reference.md";
            if (System.IO.File.Exists(docPath))
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(docPath, typeof(TextAsset)));
            }
            else
            {
                EditorUtility.DisplayDialog("Info", "API_Reference.md not found in Documentation folder.", "OK");
            }
        }

        /// <summary>
        /// Opens the platform compatibility documentation.
        /// </summary>
        [MenuItem(MenuRoot + "Documentation/Platform Compatibility")]
        public static void OpenCompatibilityGuide()
        {
            string docPath = "Assets/BatuhanAri.InventorySystem/Documentation/PLATFORM_COMPATIBILITY.md";
            if (System.IO.File.Exists(docPath))
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(docPath, typeof(TextAsset)));
            }
            else
            {
                EditorUtility.DisplayDialog("Info", "PLATFORM_COMPATIBILITY.md not found in Documentation folder.", "OK");
            }
        }

        // ==================== PRIVATE HELPERS ====================

        /// <summary>
        /// Sets up the inventory UI with Canvas and required components.
        /// </summary>
        private static void SetupInventoryUI(InventoryManager inventoryManager)
        {
            // Create Canvas
            var canvasGO = new GameObject("UI_InventoryCanvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Add Canvas Scaler
            var canvasScaler = canvasGO.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Create Background Panel
            var panelGO = new GameObject("Panel");
            panelGO.transform.SetParent(canvasGO.transform, false);
            var panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

            var panelRect = panelGO.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            // Create Inventory Window
            var windowGO = new GameObject("InventoryWindow");
            windowGO.transform.SetParent(panelGO.transform, false);
            var windowRect = windowGO.AddComponent<RectTransform>();
            windowRect.anchoredPosition = new Vector2(0, 0);
            windowRect.sizeDelta = new Vector2(400, 500);

            // Add UIInventoryWindow component
            var uiWindow = windowGO.AddComponent<UIInventoryWindow>();
            uiWindow.InventoryManager = inventoryManager;

            Debug.Log("✅ UI Canvas setup complete.");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
