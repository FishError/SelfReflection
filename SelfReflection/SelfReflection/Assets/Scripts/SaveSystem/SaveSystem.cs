using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public const string PATH = "/Saves.json";

    public static bool HasSaveFile => File.Exists(Application.dataPath + PATH);

    public static void Save(SaveData saveData)
    {
        string saveDataString = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + PATH, saveDataString);
    }

    public static void SaveLevel(SceneIndex level)
    {
        SaveData currentData = Load();
        currentData.LastVisitedLevel = level;
        Save(currentData);
    }
    
    public static SaveData Load()
    {
        CheckSaveFileIntegrity();
        string json = File.ReadAllText(Application.dataPath + PATH);
        return JsonUtility.FromJson<SaveData>(json);
    }

    private static void CheckSaveFileIntegrity()
    {
        if (HasSaveFile) return;
        SaveData newSaveData = new SaveData();
        Save(newSaveData);
    }
}
