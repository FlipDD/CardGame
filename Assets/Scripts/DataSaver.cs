using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public static class DataSaver
{
    static List<CardData> cardsData = new List<CardData>();

    public static void SaveFile(CardData data)
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) 
            file = File.OpenWrite(destination);
        else 
            file = File.Create(destination);

        var bf = new BinaryFormatter();
        cardsData.Add(data);
        bf.Serialize(file, cardsData);
        file.Close();
    }

    public static List<CardData> LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";

        if(File.Exists(destination))
        {
            var bf = new BinaryFormatter();
            var stream = new FileStream(destination, FileMode.Open);

            var data = bf.Deserialize(stream) as List<CardData>;
            foreach (var d in data)
                cardsData.Add(d);
            

            stream.Close();

            return cardsData.OrderBy(t=>t.time).ToList();
        }
        else
        {
            Debug.LogWarning("File not found");
            return null;
        }
    }
}