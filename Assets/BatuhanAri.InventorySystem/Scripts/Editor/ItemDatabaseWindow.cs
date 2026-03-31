using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BatuhanAri.InventorySystem.Data;
using BatuhanAri.InventorySystem.Core;
using System.IO;

namespace BatuhanAri.InventorySystem.Editor
{
    public class ItemDatabaseWindow : EditorWindow
    {
        private List<ItemData> allItems = new List<ItemData>();
        private Vector2 scrollPos;
        private Vector2 editorScrollPos;
        private ItemData selectedItem;
        private UnityEditor.Editor cachedEditor;
        
        [MenuItem("BatuhanAri/Item Database Wizard")]
        public static void ShowWindow()
        {
            var window = GetWindow<ItemDatabaseWindow>("Item Database");
            window.minSize = new Vector2(600, 400);
            window.Show();
        }

        private void OnEnable()
        {
            RefreshDatabase();
        }

        private void RefreshDatabase()
        {
            allItems.Clear();
            string[] guids = AssetDatabase.FindAssets("t:ItemData");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
                if (item != null)
                {
                    allItems.Add(item);
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            // Left Panel - List View
            DrawLeftPanel();

            // Right Panel - Inspector View
            DrawRightPanel();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Width(250), GUILayout.ExpandHeight(true));
            
            EditorGUILayout.LabelField("Item Database", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Create New Item"))
            {
                CreateNewItem();
            }
            if (GUILayout.Button("Refresh List"))
            {
                RefreshDatabase();
            }

            EditorGUILayout.Space(5);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var item in allItems)
            {
                if (item == null) continue;

                GUI.color = selectedItem == item ? new Color(0.7f, 0.8f, 1f) : Color.white;
                
                if (GUILayout.Button(item.Name, EditorStyles.toolbarButton))
                {
                    selectedItem = item;
                    GUI.FocusControl(null); // Remove focus to show raw updates
                }
                GUI.color = Color.white;
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawRightPanel()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (selectedItem != null)
            {
                EditorGUILayout.LabelField($"Editing: {selectedItem.Name}", EditorStyles.boldLabel);
                
                editorScrollPos = EditorGUILayout.BeginScrollView(editorScrollPos);
                
                // Draw custom editor for selected ScriptableObject
                if (cachedEditor == null || cachedEditor.target != selectedItem)
                {
                    DestroyImmediate(cachedEditor);
                    cachedEditor = UnityEditor.Editor.CreateEditor(selectedItem);
                }
                
                cachedEditor.OnInspectorGUI();

                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Space();
                if (GUILayout.Button("Delete Item", GUILayout.Height(30)))
                {
                    if (EditorUtility.DisplayDialog("Delete Item", $"Are you sure you want to delete {selectedItem.Name}?", "Delete", "Cancel"))
                    {
                        string path = AssetDatabase.GetAssetPath(selectedItem);
                        AssetDatabase.DeleteAsset(path);
                        selectedItem = null;
                        RefreshDatabase();
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select an item from the list to view and edit its properties.", MessageType.Info);
            }

            EditorGUILayout.EndVertical();
        }

        private void CreateNewItem()
        {
            string folder = "Assets/BatuhanAri.InventorySystem/Resources/Items";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            ItemData newItem = CreateInstance<ItemData>();
            newItem.Name = "New Item";
            
            string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/NewItem.asset");
            AssetDatabase.CreateAsset(newItem, path);
            AssetDatabase.SaveAssets();

            RefreshDatabase();
            selectedItem = newItem;
        }

        private void OnDestroy()
        {
            if (cachedEditor != null)
            {
                DestroyImmediate(cachedEditor);
            }
        }
    }
}
