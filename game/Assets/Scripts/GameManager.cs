using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;
    private string SavePath => Application.persistentDataPath + Path.DirectorySeparatorChar + "save.json";

    // properties
    public Save Save { get; private set; }
    public Level Level { get; set; }        // is set by MonoLevel in Awake
    public Waves Waves { get; set; }        // is set by Waves in Awake

    // constructor
    public GameManager()
    {
        if (Instance == null)
            Instance = this;
    }

    // is called when the script is being loaded
    private void Awake()
    {
        // insure that there is always just one GameManager
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Save = new Save(SavePath);

        // make this object persistent
        DontDestroyOnLoad(gameObject);
    }

    // called before the first frame
    private void Start()
    {
        MonoLevel ml = FindFirstObjectByType<MonoLevel>();

        ml.RegenerateLevel(Save.data.level, Save.data.towers);  // generate the level
        Waves.NextWave();                                       // start the first wave
    }

#if UNITY_EDITOR // debugging utility
    [ContextMenu("Set Save")]
    private void SetSave()
    {
        if (Save == null)
        {
            Debug.LogError("Can only save the file whilst running due to lack of data.");
            return;
        }

        Save.SaveFile(); // log is provided by the method itself
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
            Debug.LogError("The file doesn't exist");
        }
    }
#endif
}
