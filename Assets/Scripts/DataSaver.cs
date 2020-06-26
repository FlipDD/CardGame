using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataSaver : MonoBehaviour 
{
    int currentMoves;
    string currentName;
    float currentTime;

    // void Start()
    // {
    //     LoadFile();
    // }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) 
            file = File.OpenWrite(destination);
        else 
            file = File.Create(destination);

        var data = new CardData(currentName, currentMoves, currentTime);
        var bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        var bf = new BinaryFormatter();
        var data = (CardData) bf.Deserialize(file);
        file.Close();

        currentName = data.name;
        currentMoves = data.moves;
        currentTime = data.time;

        Debug.Log(data.name);
        Debug.Log(data.moves);
        Debug.Log(data.time);
    }
}