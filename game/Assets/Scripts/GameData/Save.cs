using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save
{
    public readonly string savePath;    // save path is set externally because Unity doesn't like you touching persistentDataPath in constructors
    public SaveData data;               // contains the save data (duh)

    // acquires the latest safe data!
    private void UpdateSaveData()
    {
        Level level = GameManager.Instance.Level;
        IReadOnlyList<Vector2Int> towers = level.GetTowers();

        // assign a new array for the towers data
        data.towers = new TileData[towers.Count];

        // set the towers in the array
        for (int i = 0; i < towers.Count; i++)
            data.towers[i] = level.GetTile(towers[i]);
    }

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
        UpdateSaveData();

        // get how long it took to perform this action
        string json = JsonUtility.ToJson(data); // convert the save data to JSON
        File.WriteAllText(savePath, json);      // write the serialized JSON data to the file

        Debug.Log($"saved the data to '{savePath}'.");
    }
}
