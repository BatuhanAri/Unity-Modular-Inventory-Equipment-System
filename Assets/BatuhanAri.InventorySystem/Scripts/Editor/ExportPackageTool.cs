using UnityEditor;
using UnityEngine;
using System.IO;

namespace BatuhanAri.InventorySystem.Editor
{
    public class ExportPackageTool
    {
        [MenuItem("BatuhanAri/Export For Asset Store")]
        public static void ExportAssetStorePackage()
        {
            string folderPath = "Assets/BatuhanAri.InventorySystem";
            string exportPath = "BatuhanAri_InventorySystem_v1.0.unitypackage";

            if (Directory.Exists(folderPath))
            {
                // Ensure everything is saved
                AssetDatabase.SaveAssets();

                // Export package
                AssetDatabase.ExportPackage(folderPath, exportPath, ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
                
                // Show in explorer
                EditorUtility.RevealInFinder(exportPath);
                
                Debug.Log($"Successfully exported Asset Store package to: {exportPath}");
            }
            else
            {
                Debug.LogError($"Cannot find the folder to export at: {folderPath}. Please make sure the folder structure hasn't been changed manually.");
            }
        }
    }
}
