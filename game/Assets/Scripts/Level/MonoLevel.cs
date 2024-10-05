using UnityEngine;

public class MonoLevel : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 10;    // non-negative value corresponding to the maximum amount of tiles on the horizontal axis
    [SerializeField, Min(1)] private int height = 10;   // non-negative value corresponding to the maximum amount of tiles on the vertucal axis
    [SerializeField] private MonoTile pathPrefab;       // contains the prefab which contains a singular node of the path

    public Level level = null;

    // called when the script is being loaded
    private void Awake()
    {
        level = new Level(width, height);
        level.GenerateLevel(new System.Random().Next());

        // generate the path
        foreach (Vector2Int pathPos in level.GetPath())
        {
            MonoTile tile = Instantiate(pathPrefab, new Vector3(pathPos.x + 0.5F, 0, pathPos.y + 0.5F), Quaternion.identity, transform);
            tile.SetLevel(level);
            tile.SetTilePos(pathPos);
        }
    }

# if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(new Vector3(width / 2, 0, height / 2), new Vector3(width, 0, height));
    }
#endif
}
