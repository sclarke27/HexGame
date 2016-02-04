using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class FileManager
{

    private static string filePath = Application.persistentDataPath + "/";
    private static string fileName = "savedGames.gd";
    private static List<SerializableMap> savedMaps = new List<SerializableMap>();

    public static void SaveAtIndex(int index, SerializableMap mapData)
    {
        savedMaps[index] = mapData;
        WriteSaveFile();
    }

    public static void SaveNew(SerializableMap mapData)
    {
        FileManager.savedMaps.Add(mapData);
        WriteSaveFile();
    }

    private static void WriteSaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(filePath + fileName); 
        try
        {
            bf.Serialize(fileStream, FileManager.savedMaps);
        }
        catch (IOException ex)
        {
            Debug.LogError(ex);
        }
        fileStream.Close();
        CacheMapData();
    }

    public static SerializableMap LoadMapData(int mapIndex)
    {
        return FileManager.savedMaps[mapIndex];
    }

    public static int LoadedMapCount()
    {
        return (FileManager.savedMaps.Count > 0) ? FileManager.savedMaps.Count - 1 : 0;
    }

    public static void CacheMapData()
    {
        FileStream fileStream = File.Open(filePath + fileName, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        try {
            if (File.Exists(filePath + fileName))
            {
                FileManager.savedMaps = (List<SerializableMap>)bf.Deserialize(fileStream);
                Debug.Log("Cached " + FileManager.savedMaps.Count.ToString() + " saved maps.");
            }
        }
        catch(IOException ex)
        {
            Debug.LogError(ex);
        }
        fileStream.Close();
    }

    public static List<SerializableMap> CachedMapList
    {
        get { return FileManager.savedMaps; }
    }
}