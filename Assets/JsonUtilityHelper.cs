using System.IO;
using UnityEngine;

public static class JsonUtilityHelper
{
    public static void SaveToFile(string fileName, string json)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"Data saved to {path}");
    }

    public static string LoadFromFile(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log($"Data loaded from {path}");
            return json;
        }
        else
        {
            Debug.LogWarning($"File {path} not found.");
            return null;
        }
    }
}
