using System;
using UnityEngine;

public class MonoLevel : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 10;        // non-negative value corresponding to the maximum amount of tiles on the horizontal axis
    [SerializeField, Min(1)] private int height = 10;       // non-negative value corresponding to the maximum amount of tiles on the vertucal axis

    // the prefabs containing the path models
    [SerializeField] private TilePrefabData pathPrefabs;    // contains the path prefabs

    [Obsolete("use Level, not level")] public Level level => Level; // for compatibility reasons
    public Level Level { get; private set; }

    // selects which prefab to use for the path tile depending on the tile position & level state, and sets it in the Unity scene
    private void SetPath(Vector2Int pos)
    {
        // get the empty neighbors as a binary string (basically a bunch of boolean values)
        // 1=occupied, 0=not occupied / 1=true, 0=false
        // In binary, from left to right, the first two bits/booleans represent whether there is a neighbour at positive X, then Y
        // the last two represent negative X, then Y.
        byte neighbors = 0;
        if (!Level.IsValidPlacement(pos.x + 1, pos.y, true)) neighbors |= 0b1000;   // +X
        if (!Level.IsValidPlacement(pos.x, pos.y + 1, true)) neighbors |= 0b0100;   // +Y
        if (!Level.IsValidPlacement(pos.x - 1, pos.y, true)) neighbors |= 0b0010;   // -X
        if (!Level.IsValidPlacement(pos.x, pos.y - 1, true)) neighbors |= 0b0001;   // -Y

        // get the data for which prefab to use and what rotation
        (GameObject obj, float rot) data = neighbors switch
        {
            0b0000 => (pathPrefabs.full, 0.0F),         // tile has no neighbours
            0b1111 => (pathPrefabs.empty, 0.0F),        // tile has all neighbours
            0b0101 => (pathPrefabs.straight, 0.0F),     // tile has neighbours in the Y direction, (+Y, -Y)
            0b1010 => (pathPrefabs.straight, 90.0F),    // tile has neighbours in the X direction, (+X, -X)
            0b1100 => (pathPrefabs.corner, -90.0F),     // +X, +Y
            0b1001 => (pathPrefabs.corner, 180.0F),     // +X, -Y
            0b0011 => (pathPrefabs.corner, 90.0F),      // -X, -Y
            0b0110 => (pathPrefabs.corner, 0.0F),       // -X, +Y
            0b0111 => (pathPrefabs.single, 0.0F),       // only +X is free
            0b1011 => (pathPrefabs.single, 90.0F),      // only +Y is free
            0b1101 => (pathPrefabs.single, 180.0F),     // only -X is free
            0b1110 => (pathPrefabs.single, -90.0F),     // only -Y is free

            // if there is just a singular neighbor, just return the empty path type
            0b1000 => (pathPrefabs.empty, 0.0F),
            0b0100 => (pathPrefabs.empty, 0.0F),
            0b0010 => (pathPrefabs.empty, 0.0F),
            0b0001 => (pathPrefabs.empty, 0.0F),

            // if it's none of these options, throw an error, with the binary string
            _ => throw new Exception($"invalid state: 0b{Convert.ToString(neighbors, 2).PadLeft(4, '0')}"),
        };

        // set the path at the position with the given rotation
        SetPath(data.obj, pos, data.rot);
    }

    // sets the path "tile" using the specified prefab with the specified Y rotation, in the unity scene
    private void SetPath(GameObject prefab, Vector2Int pos, float rotation)
    {
        GameObject obj = Instantiate(prefab, new Vector3(pos.x + 0.5F, 0, pos.y + 0.5F), Quaternion.Euler(-90.0F, rotation, 0.0F), transform);
        obj.name = pos.ToString();
        MonoTile tile = obj.AddComponent<MonoTile>();
        tile.Initialize(Level, pos, pathPrefabs);
    }

    // called when the script is being loaded
    private void Awake()
    {
        Level = new Level(width, height);
        Level.GenerateLevel(new System.Random().Next());

        // generate the path
        foreach (Vector2Int pathPos in Level.GetPath())
        {
            SetPath(pathPos);
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
