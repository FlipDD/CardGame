using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public static class DataSaver
{
    static List<RoundData> roundsData = new List<RoundData>();

    public static void SaveFile(RoundData data)
    {
        Debug.Log("Saved file");

        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) 
            file = File.OpenWrite(destination);
        else 
            file = File.Create(destination);

        var bf = new BinaryFormatter();
        roundsData.Add(data);
        bf.Serialize(file, roundsData);
        file.Close();
    }

    public static List<RoundData> LoadFile()
    {
        Debug.Log("Loaded file");

        string destination = Application.persistentDataPath + "/save.dat";

        if(File.Exists(destination))
        {
            var bf = new BinaryFormatter();
            var stream = new FileStream(destination, FileMode.Open);

            var data = bf.Deserialize(stream) as List<RoundData>;

            roundsData = data;
            // foreach (var d in data)
            //     roundsData.Add(d);
            
            stream.Close();

            return roundsData.OrderBy(t=>t.time).ToList();
        }
        else
        {
            Debug.LogWarning("File not found");
            return null;
        }
    }
}