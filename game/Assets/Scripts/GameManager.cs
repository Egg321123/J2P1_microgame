using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    // properties
    public Save Save { get; private set; }
    public Level Level { get; set; }

    // is called when the script is being loaded
    private void Awake()
    {
        // insure that there is always just one GameManager
        if (Instance != null)
            Destroy(gameObject);

        Save = new Save(Application.persistentDataPath + Path.DirectorySeparatorChar + "save.json");
        instance = this;

        // make this object persistent
        DontDestroyOnLoad(gameObject);
    }
}
