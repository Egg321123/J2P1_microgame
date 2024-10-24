using System;
using UnityEngine;

public class MonoTile : MonoBehaviour
{
    [SerializeField] private TileMeshData tileMeshes;
    private bool initialized = false;
    private Level level;
    private Vector2Int tilePos;
    private MeshFilter meshFilter = null;

    // readonly public fields
    public TileData Data => level.GetTile(tilePos);
    public Level Level => level;
    public Vector2Int TilePos => tilePos;

    private void UpdateModel(bool updateNeighbours = false) => SetModel(tilePos, updateNeighbours);

    private void SetModel(Vector2Int pos, bool updateNeighbours = false)
    {
        // get the relative positions
        Vector2Int north = new(pos.x + 1, pos.y);
        Vector2Int east = new(pos.x, pos.y + 1);
        Vector2Int south = new(pos.x - 1, pos.y);
        Vector2Int west = new(pos.x, pos.y - 1);

        // get the empty neighbors as a binary string (basically a bunch of boolean values)
        // 1=occupied, 0=not occupied / 1=true, 0=false
        // In binary, from left to right, the first two bits/booleans represent whether there is a neighbour at positive X, then Y
        // the last two represent negative X, then Y.
        byte neighbors = 0;
        if (!level.IsEmpty(north, true)) neighbors |= 0b1000;   // +X
        if (!level.IsEmpty(east, true)) neighbors |= 0b0100;    // +Y
        if (!level.IsEmpty(south, true)) neighbors |= 0b0010;   // -X
        if (!level.IsEmpty(west, true)) neighbors |= 0b0001;    // -Y

        // get the data for which prefab to use and what rotation
        (Mesh mesh, float rotation) data = neighbors switch
        {
            0b0000 => (tileMeshes.full, 0.0F),          // tile has no neighbours
            0b1111 => (tileMeshes.empty, 0.0F),         // tile has all neighbours
            0b0101 => (tileMeshes.straight, 90.0F),     // tile has neighbours in the Y direction, (+Y, -Y)
            0b1010 => (tileMeshes.straight, 0.0F),      // tile has neighbours in the X direction, (+X, -X)
            0b1100 => (tileMeshes.corner, 90.0F),       // +X, +Y
            0b1001 => (tileMeshes.corner, 180.0F),      // +X, -Y
            0b0011 => (tileMeshes.corner, -90.0F),      // -X, -Y
            0b0110 => (tileMeshes.corner, 0.0F),        // -X, +Y
            0b0111 => (tileMeshes.single, -90.0F),      // only +X is free
            0b1011 => (tileMeshes.single, 180.0F),      // only +Y is free
            0b1101 => (tileMeshes.single, 90.0F),       // only -X is free
            0b1110 => (tileMeshes.single, 0.0F),        // only -Y is free

            // if there is just a singular neighbor, just return the empty path type
            0b1000 => (tileMeshes.end, 0.0F),           // neighbour at +X
            0b0100 => (tileMeshes.end, -90.0F),         // neighbour at +Y
            0b0010 => (tileMeshes.end, 180.0F),         // neighbour at -X
            0b0001 => (tileMeshes.end, 90.0F),          // neighbour at -Y

            // if it's none of these options; throw an error, with the binary string
            _ => throw new Exception($"invalid state: 0b{Convert.ToString(neighbors, 2).PadLeft(4, '0')}"),
        };

        SetModel(data.mesh, data.rotation);

        if (updateNeighbours == false)
            return;

        // check if a tile has been set, update the model of that tile
        if ((neighbors & 0b1000) != 0) level.GetTile(north).monoTile.UpdateModel();
        if ((neighbors & 0b0100) != 0) level.GetTile(east).monoTile.UpdateModel();
        if ((neighbors & 0b0010) != 0) level.GetTile(south).monoTile.UpdateModel();
        if ((neighbors & 0b0001) != 0) level.GetTile(west).monoTile.UpdateModel();
    }

    private void SetModel(Mesh mesh, float rotation)
    {
        meshFilter ??= GetComponent<MeshFilter>();  // get mesh filter component if we haven't already
        meshFilter.mesh = mesh;

        transform.rotation = Quaternion.Euler(0, rotation, 0.0F);
    }

    /// <summary>
    /// initializes the object, throws <see cref="InvalidOperationException"/> when already initialized
    /// </summary>
    public void Initialize(Level level, Vector2Int tilePos, bool updateNeighbours = false)
    {
        if (initialized == true) throw new InvalidOperationException("this object has already been initialized!");
        initialized = true;

        // set fields
        this.level = level;
        this.tilePos = tilePos;
        transform.position = new Vector3(tilePos.x + 0.5F, 0.0F, tilePos.y + 0.5F);

        // assign self to the mono tile
        level.tiles[tilePos.x, tilePos.y].monoTile = this;

        SetModel(tilePos, updateNeighbours);

        Initialize();
    }

    // in the case that some extra initialisation logic needs to be implemented. :3
    protected virtual void Initialize() { }
}
