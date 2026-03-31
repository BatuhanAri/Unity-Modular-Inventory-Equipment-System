namespace ModularInventory.SaveSystem
{
    /// <summary>
    /// Abstract interface for any Save/Load provider (e.g. JSON in PlayerPrefs, EasySave, BinaryFormatter, Cloud Save)
    /// </summary>
    public interface ISaveProvider
    {
        void Save(string key, string jsonData);
        string Load(string key);
        bool HasSave(string key);
    }
}
