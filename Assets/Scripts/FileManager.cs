using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
 
public static class FileManager
{

    public static List<SerializableMap> savedGames = new List<SerializableMap>();

    //it's static so we can call it from anywhere
    public static void Save(SerializableMap mapData)
    {
        FileManager.savedGames.Add(mapData);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        try
        {
            bf.Serialize(file, FileManager.savedGames);
        }
        catch(IOException ex)
        {
            Debug.LogError(ex);
        }
        file.Close();
        Debug.Log(Application.persistentDataPath + "/savedGames.gd");
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
        FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        try {
            if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
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