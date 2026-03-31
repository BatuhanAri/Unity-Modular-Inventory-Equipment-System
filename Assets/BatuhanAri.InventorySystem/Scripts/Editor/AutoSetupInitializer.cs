using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

namespace BatuhanAri.InventorySystem.Editor
{
    /// <summary>
    /// Automated initialization script for creating prefabs and demo scene.
    /// Called via Unity -executeMethod from command line.
    /// </summary>
    public class AutoSetupInitializer
    {
        private static string PrefabFolder = "Assets/BatuhanAri.InventorySystem/Prefabs";
        private static string DemoScenePath = "Assets/BatuhanAri.InventorySystem/Demo/InventoryDemo.unity";

        /// <summary>
        /// Main entry point for automated setup. Call via:
        /// Unity -projectPath <path> -executeMethod BatuhanAri.InventorySystem.Editor.AutoSetupInitializer.RunAutoSetup -quit -batchmode
        /// </summary>
        public static void RunAutoSetup()
        {
            Debug.Log("🚀 Starting automatic inventory system setup...");

            try
            {
                // Create directories
                CreateDirectoriesIfNeeded();

                // Create prefabs
                CreateInventoryManagerPrefab();
                Debug.Log("✅ InventoryManager prefab created");

                CreateEquipmentManagerPrefab();
                Debug.Log("✅ EquipmentManager prefab created");

                CreateUICanvasPrefab();
                Debug.Log("✅ UI Canvas prefab created");

                // Create demo scene
                CreateDemoScene();
                Debug.Log("✅ Demo scene created");

                // Save assets
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("🎉 Automatic setup completed successfully!");
                Debug.Log("📍 Check the following locations:");
                Debug.Log($"   - Prefabs: {PrefabFolder}");
                Debug.Log($"   - Demo Scene: {DemoScenePath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Setup failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static void CreateDirectoriesIfNeeded()
        {
            if (!AssetDatabase.IsValidFolder(PrefabFolder))
            {
                string parentFolder = "Assets/BatuhanAri.InventorySystem";
                AssetDatabase.CreateFolder(parentFolder, "Prefabs");
                Debug.Log($"Created folder: {PrefabFolder}");
            }

            string demoFolder = "Assets/BatuhanAri.InventorySystem/Demo";
            if (!AssetDatabase.IsValidFolder(demoFolder))
            {
                AssetDatabase.CreateFolder("Assets/BatuhanAri.InventorySystem", "Demo");
                Debug.Log($"Created folder: {demoFolder}");
            }
        }

        private static void CreateInventoryManagerPrefab()
        {
            var go = new GameObject("InventoryManager");
            var inventoryManager = go.AddComponent<InventoryManager>();
            inventoryManager.InitializeInventory(20);

            string prefabPath = $"{PrefabFolder}/InventoryManager.prefab";
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        private static void CreateEquipmentManagerPrefab()
        {
            var go = new GameObject("EquipmentManager");
            go.AddComponent<EquipmentManager>();

            string prefabPath = $"{PrefabFolder}/EquipmentManager.prefab";
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        private static void CreateUICanvasPrefab()
        {
            // Create Canvas
            var canvasGO = new GameObject("UI_InventoryCanvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Add Canvas Scaler
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // Add Graphics Raycaster
            canvasGO.AddComponent<GraphicRaycaster>();

            // Create Background
            var bgGO = new GameObject("Background");
            bgGO.transform.SetParent(canvasGO.transform, false);
            var bgImage = bgGO.AddComponent<Image>();
            bgImage.color = new Color(0, 0, 0, 0.5f);
            var bgRect = bgGO.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            // Create Inventory Panel
            var panelGO = new GameObject("InventoryPanel");
            panelGO.transform.SetParent(canvasGO.transform, false);
            var panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.95f);
            var panelRect = panelGO.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(500, 600);
            panelRect.anchoredPosition = Vector2.zero;

            // Add UIInventoryWindow if it exists
            try
            {
                var uiWindow = panelGO.AddComponent<UIInventoryWindow>();
                Debug.Log("UIInventoryWindow component added to prefab");
            }
            catch
            {
                Debug.LogWarning("UIInventoryWindow component not found, skipping");
            }

            // Save as prefab
            string prefabPath = $"{PrefabFolder}/UI_Canvas.prefab";
            PrefabUtility.SaveAsPrefabAsset(canvasGO, prefabPath);
            Object.DestroyImmediate(canvasGO);
        }

        private static void CreateDemoScene()
        {
            // Create new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // Load prefabs and instantiate
            var inventoryPrefab = AssetDatabase.LoadAssetAtPath($"{PrefabFolder}/InventoryManager.prefab", typeof(GameObject)) as GameObject;
            var equipmentPrefab = AssetDatabase.LoadAssetAtPath($"{PrefabFolder}/EquipmentManager.prefab", typeof(GameObject)) as GameObject;
            var uiPrefab = AssetDatabase.LoadAssetAtPath($"{PrefabFolder}/UI_Canvas.prefab", typeof(GameObject)) as GameObject;

            if (inventoryPrefab != null)
            {
                var inventoryInstance = PrefabUtility.InstantiatePrefab(inventoryPrefab) as GameObject;
                inventoryInstance.name = "InventoryManager";
            }

            if (equipmentPrefab != null)
            {
                var equipmentInstance = PrefabUtility.InstantiatePrefab(equipmentPrefab) as GameObject;
                equipmentInstance.name = "EquipmentManager";
            }

            if (uiPrefab != null)
            {
                var uiInstance = PrefabUtility.InstantiatePrefab(uiPrefab) as GameObject;
                uiInstance.name = "UI_Canvas";

                // Try to connect InventoryManager to UI
                var inventoryManager = FindObjectOfType<InventoryManager>();
                if (inventoryManager != null)
                {
                    var uiWindow = uiInstance.GetComponentInChildren<UIInventoryWindow>();
                    if (uiWindow != null)
                    {
                        uiWindow.InventoryManager = inventoryManager;
                        Debug.Log("Connected InventoryManager to UI");
                    }
                }
            }

            // Save scene
            EditorSceneManager.SaveScene(scene, DemoScenePath);
            Debug.Log($"Demo scene saved to: {DemoScenePath}");
        }
    }
}
