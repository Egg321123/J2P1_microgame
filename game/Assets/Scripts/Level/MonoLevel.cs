using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoLevel : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 10;        // non-negative value corresponding to the maximum amount of tiles on the horizontal axis
    [SerializeField, Min(1)] private int height = 10;       // non-negative value corresponding to the maximum amount of tiles on the vertucal axis
    [SerializeField] private MonoTile pathPrefab;           // contains the path prefabs
    private readonly List<MonoTile> placedTiles = new();    // contains the tiles that have been placed

    private CreateAIPath pathCreator = null;                // reference to the path creator for the enemies

    public Level Level { get; private set; }

    private void DestroyLevel()
    {
        // destroy all the placed tiles
        foreach (MonoTile tile in placedTiles)
        {
            Destroy(tile.gameObject);
        }

        // clear the placed tiles list
        placedTiles.Clear();
    }

    public void RegenerateLevel(int level, TileData[] towers)
    {
        DestroyLevel();
        Level.GenerateLevel(level); // generate the level

        // generate the path monotiles
        foreach (Vector2Int pathPos in Level.GetPath())
        {
            SetMonoTile(pathPrefab, pathPos);
        }

        foreach (TileData tower in towers)
        {
            Level.SetTile(tower.pos, TileType.TOWER, tower.towerData);
            SetMonoTile(tower.monoTile, tower.pos, true);
        }

        // regenerate the AI path, so enemies know where to walk
        pathCreator.RegeneratePath();
    }

    /// <summary>
    /// sets a tile in <see cref="Level"/> and in the unity scene
    /// </summary>
    public void SetTile(MonoTile prefab, Vector2Int pos, TileType tileType, TowerData? towerData = null, bool updateNeighbours = false)
    {
        Level.SetTile(pos, tileType, towerData);
        SetMonoTile(prefab, pos, updateNeighbours);
    }

    /// <summary>
    /// initialize the monoTile prefab
    /// </summary>
    public void SetMonoTile(MonoTile prefab, Vector2Int pos, bool updateNeighbours = false)
    {
        MonoTile tile = Instantiate(prefab, transform);
        tile.name = pos.ToString();
        tile.Initialize(Level, pos, updateNeighbours);
        placedTiles.Add(tile);
    }

    // called when the script is being loaded
    private void Awake()
    {
        pathCreator = FindFirstObjectByType<CreateAIPath>();

        if (GameManager.Instance.Level == null)
        {
            Level = new Level(width, height);
            GameManager.Instance.Level = Level;
        }
        else
            Level = GameManager.Instance.Level;
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
