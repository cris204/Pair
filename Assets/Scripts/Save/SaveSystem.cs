using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveGame(SavedGameState state)
    {
        string json = JsonUtility.ToJson(state, true);
        File.WriteAllText(SavePath, json);
    }

    public static SavedGameState LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            return null;
        }
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<SavedGameState>(json);
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
    }
}
