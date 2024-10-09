using System.IO;
using UnityEngine;

public class Save
{
    public readonly string savePath;
    public SaveData data;

    /// <summary>
    /// creates a new instance of Save, either with <see cref="data"/> set to their default values or what is stored at <see cref="savePath"/>
    /// </summary>
    public Save(string savePath)
    {
        this.savePath = savePath;

        if (File.Exists(savePath))
        {
            Debug.Log($"loading the data from '{savePath}'...");

            // initialize the data from JSON
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log($"loaded data: '{json}'");
            return;
        }

        // initialize the data with default values
        data = new();
        data.Initialize();
    }

    /// <summary>
    /// serializes <see cref="data"/> to JSON and stores it at the file at <see cref="savePath"/>
    /// </summary>
    public void SaveFile()
    {
        // get how long it took to perform this action
        string json = JsonUtility.ToJson(data); // convert the save data to JSON
        File.WriteAllText(savePath, json);      // write the serialized JSON data to the file

        Debug.Log($"saved the data to '{savePath}'.");
    }
}
