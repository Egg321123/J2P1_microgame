using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance => instance;
    private string SavePath => Application.persistentDataPath + Path.DirectorySeparatorChar + "save.json";

    // properties
    public Save Save { get; private set; }
    public Level Level { get; set; }        // is set by MonoLevel in Awake
    public Waves Waves { get; set; }        // is set by Waves in Awake

    // constructor
    public GameManager()
    {
        if (instance == null)
            instance = this;

    }

    // is called when the script is being loaded
    private void Awake()
    {
        // insure that there is always just one GameManager
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Save = new Save(SavePath);

        // make this object persistent
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MonoLevel ml = FindFirstObjectByType<MonoLevel>();
        ml.RegenerateLevel(Save.data.level, Save.data.towers);  // generate the level
        Waves.NextWave();                                       // start the first wave
    }

#if UNITY_EDITOR
    [ContextMenu("Set Save")]
    private void SetSave()
    {
        Save.SaveFile();
    }

    [ContextMenu("Reset Save")]
    private void ResetSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Removed the save file");
        }
        else
        {
            Debug.Log("The file doesn't exist");
        }
    }
#endif
}
