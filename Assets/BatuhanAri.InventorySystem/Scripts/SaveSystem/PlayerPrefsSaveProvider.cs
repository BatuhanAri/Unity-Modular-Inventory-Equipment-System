using UnityEngine;

namespace BatuhanAri.InventorySystem.SaveSystem
{
    /// <summary>
    /// Built-in save provider that uses Unity's PlayerPrefs class.
    /// Good for rapid prototyping and local games without sensitive data.
    /// </summary>
    public class PlayerPrefsSaveProvider : MonoBehaviour, ISaveProvider
    {
        public void Save(string key, string jsonData)
        {
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public string Load(string key)
        {
            return PlayerPrefs.GetString(key, string.Empty);
        }

        public bool HasSave(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}

