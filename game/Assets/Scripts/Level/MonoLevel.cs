using System;
using UnityEngine;

public class MonoLevel : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 10;        // non-negative value corresponding to the maximum amount of tiles on the horizontal axis
    [SerializeField, Min(1)] private int height = 10;       // non-negative value corresponding to the maximum amount of tiles on the vertucal axis

    // the prefabs containing the path models
    [SerializeField] private MonoTile pathPrefab;    // contains the path prefabs

    [Obsolete("use Level, not level")] public Level level => Level; // for compatibility reasons
    public Level Level { get; private set; }

    // called when the script is being loaded
    private void Awake()
    {
        Level = new Level(width, height);
        Level.GenerateLevel(new System.Random().Next());

        // generate the path
        foreach (Vector2Int pathPos in Level.GetPath())
        {
            MonoTile tile = Instantiate(pathPrefab, transform);
            tile.Initialize(Level, pathPos);
        }
    }

#if UNITY_EDITOR
    // draw level bounds for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(new Vector3(width / 2, 0, height / 2), new Vector3(width, 0, height));
    }
#endif
}
