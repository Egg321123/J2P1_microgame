using UnityEngine;

public class MonoLevel : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 10;    // non-negative value corresponding to the maximum amount of tiles on the horizontal axis
    [SerializeField, Min(1)] private int height = 10;   // non-negative value corresponding to the maximum amount of tiles on the vertucal axis

    // the prefabs containing the path models
    [SerializeField] private MonoTile pathPrefab;       // contains the path prefabs

    public Level Level { get; private set; }

    /// <summary>
    /// sets a tile in <see cref="Level"/> and in the unity scene
    /// </summary>
    public void SetTile(MonoTile prefab, Vector2Int pos, TileType tileType, TowerData? towerData = null, bool updateNeighbours = false)
    {
        Level.SetTile(pos, TileType.PATH, towerData);
        SetMonoTile(prefab, pos);
    }

    /// <summary>
    /// initialize the monoTile prefab
    /// </summary>
    public void SetMonoTile(MonoTile prefab, Vector2Int pos, bool updateNeighbours = false)
    {
        MonoTile tile = Instantiate(prefab, transform);
        tile.Initialize(Level, pos, updateNeighbours);
    }

    // called when the script is being loaded
    private void Awake()
    {
        Level = new Level(width, height);
        Level.GenerateLevel(new System.Random().Next());

        // generate the path
        foreach (Vector2Int pathPos in Level.GetPath())
        {
            SetMonoTile(pathPrefab, pathPos);
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
