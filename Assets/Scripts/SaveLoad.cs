using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{

    public static Game savedGame;
    public static string fileName = "yourProgress";
    public static string fileExtention = "fy";
    
    public static void Save()
    {
        savedGame = Game.current;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName + "." + fileExtention);
        bf.Serialize(file, savedGame);
        file.Close();
    }

    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName + "." + fileExtention))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + "." + fileExtention, FileMode.Open);
            savedGame = (Game)bf.Deserialize(file);
            Game.current = savedGame;
            file.Close();
            return true;
        }
        else { return false; }
    }
}
