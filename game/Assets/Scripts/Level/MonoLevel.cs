using System;
using System.ComponentModel;
using UnityEngine;

public class MonoLevel : MonoBehaviour
{
    [SerializeField, Min(1)] private int width = 10;    // non-negative value corresponding to the maximum amount of tiles on the horizontal axis
    [SerializeField, Min(1)] private int height = 10;   // non-negative value corresponding to the maximum amount of tiles on the vertucal axis

    // the prefabs containing the path models
    [Header("Path Prefabs")]
    [SerializeField] private GameObject pathStraight;
    [SerializeField] private GameObject pathCorner;
    [SerializeField] private GameObject pathSingle;
    [SerializeField] private GameObject pathFull;
    [SerializeField] private GameObject pathEmpty;

    public Level level = null;

    private void SetPath(Vector2Int pos)
    {
        // get the empty neighbors as a binary string
        // 1: occupied, 0: not occupied
        // from left to right, first positive X, Y then negative X, Y
        byte neighbors = 0;
        if (!level.IsValidPlacement(pos.x + 1, pos.y, true)) neighbors |= 1 << 0; // use bitwise OR operator to add 1 in the integer
        if (!level.IsValidPlacement(pos.x, pos.y + 1, true)) neighbors |= 1 << 1; // the bitwise shift operator is used to shift it that many "booleans" to the left
        if (!level.IsValidPlacement(pos.x - 1, pos.y, true)) neighbors |= 1 << 2;
        if (!level.IsValidPlacement(pos.x, pos.y - 1, true)) neighbors |= 1 << 3;

        (GameObject obj, float rot) data = neighbors switch
        {
            0b0000 => (pathFull, 0.0F),         // tile has no neighbours
            0b1111 => (pathEmpty, 0.0F),        // tile has all neighbours
            0b0101 => (pathStraight, 0.0F),     // tile has neighbours in the Y direction
            0b1010 => (pathStraight, 90.0F),    // tile has neighbours in the X direction
            0b1100 => (pathCorner, -90.0F),     // +X +Y
            0b1001 => (pathCorner, 180.0F),     // +X -Y
            0b0011 => (pathCorner, 90.0F),      // -X -Y
            0b0110 => (pathCorner, 0.0F),       // -X +Y
            0b0111 => (pathSingle, 0.0F),       // only +X is free
            0b1011 => (pathSingle, 90.0F),      // only +Y is free
            0b1101 => (pathSingle, 180.0F),     // only -X is free
            0b1110 => (pathSingle, -90.0F),     // only -Y is free
            0b1000 => (pathEmpty, 0.0F),        // UwU
            0b0100 => (pathEmpty, 0.0F),        // UwU
            0b0010 => (pathEmpty, 0.0F),        // UwU
            0b0001 => (pathEmpty, 0.0F),        // UwU
            _ => throw new Exception($"invalid state: 0b{Convert.ToString(neighbors, 2).PadLeft(4, '0')}"),
        };

        SetPath(data.obj, pos, new Vector3(-90.0F, data.rot, 0.0F));
    }

    private void SetPath(GameObject prefab, Vector2Int pos, Vector3 rotation)
    {
        GameObject obj = Instantiate(prefab, new Vector3(pos.x + 0.5F, 0, pos.y + 0.5F), Quaternion.Euler(rotation), transform);
        MonoTile tile = obj.AddComponent<MonoTile>();
        tile.SetLevel(level);
        tile.SetTilePos(pos);
    }

    // called when the script is being loaded
    private void Awake()
    {
        level = new Level(width, height);
        level.GenerateLevel(new System.Random().Next());

        // generate the path
        foreach (Vector2Int pathPos in level.GetPath())
        {
            SetPath(pathPos);
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
