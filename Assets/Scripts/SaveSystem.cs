using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/**
* @memo 2022
* save system script
*/
public class SaveSystem
{
    /**
* @memo 2022
* saves data in file
*/
    public static void SavePlayer(MainController Controller)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.mpim";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(Controller);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    /**
* @memo 2022
* loads data from file
*/
    public static SaveData loadPlayer()
    {
        string path = Application.persistentDataPath + "/save.mpim";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
