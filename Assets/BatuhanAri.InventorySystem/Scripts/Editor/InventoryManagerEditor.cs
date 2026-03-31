using UnityEditor;
using UnityEngine;
using BatuhanAri.InventorySystem.Inventory;

namespace BatuhanAri.InventorySystem.Editor
{
    [CustomEditor(typeof(InventoryManager))]
    public class InventoryManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw default fields
            DrawDefaultInspector();

            InventoryManager manager = (InventoryManager)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("Debug Tools (Play Mode Only)", MessageType.Info);

            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Clear Entire Inventory"))
            {
                if (EditorUtility.DisplayDialog("Clear Inventory", "Are you sure you want to delete all items inside the inventory?", "Yes", "Cancel"))
                {
                    manager.ClearInventory();
                }
            }

            // Quick display of fill status
            if (Application.isPlaying && manager.GetAllSlots() != null)
            {
                int filledSlots = 0;
                foreach (var slot in manager.GetAllSlots())
                {
                    if (!slot.IsEmpty) filledSlots++;
                }
                
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField($"Status: {filledSlots} / {manager.MaxSlots} slots filled", EditorStyles.boldLabel);
            }
            GUI.enabled = true;
        }
    }
}

