using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class FileManager
{

    private static string filePath = Application.persistentDataPath + "/";
    private static string fileName = "savedGames.gd";
    public static List<SerializableMap> savedGames = new List<SerializableMap>();

    public static void SaveAtIndex(int index, SerializableMap mapData)
    {

    }

    //it's static so we can call it from anywhere
    public static void SaveNew(SerializableMap mapData)
    {
        FileManager.savedGames.Add(mapData);
        WriteSaveFile();

        Debug.Log(filePath + fileName);
    }

    private static void WriteSaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath + fileName); //you can call it anything you want
        try
        {
            bf.Serialize(file, FileManager.savedGames);
        }
        catch (IOException ex)
        {
            Debug.LogError(ex);
        }
        file.Close();
        CacheMapData();
    }

    public static SerializableMap LoadMapData(int mapIndex)
    {
        return FileManager.savedGames[mapIndex];
    }

    public static int LoadedMapCount()
    {
        return (FileManager.savedGames.Count > 0) ? FileManager.savedGames.Count - 1 : 0;
    }

    public static void CacheMapData()
    {
        FileStream file = File.Open(filePath + fileName, FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        try {
            if (File.Exists(filePath + fileName))
            {
                FileManager.savedGames = (List<SerializableMap>)bf.Deserialize(file);
                Debug.Log("Cached " + FileManager.savedGames.Count.ToString() + " saved maps.");
            }
        }
        catch(IOException ex)
        {
            Debug.LogError(ex);
        }
        file.Close();
    }
}