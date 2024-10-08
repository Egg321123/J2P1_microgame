using System;
using UnityEngine;

public class MonoTile : MonoBehaviour
{
    private bool initialized = false;
    private Level level;
    private Vector2Int tilePos;
    private TilePrefabData tilePrefabs;

    public TileData Data => level.GetTile(tilePos);

    /// <summary>
    /// initializes the object, throws <see cref="InvalidOperationException"/> when already set
    /// </summary>
    public void Initialize(Level level, Vector2Int tilePos, TilePrefabData tilePrefabs)
    {
        if (initialized != false) throw new InvalidOperationException("attempted to set level whilst level was already set!");
        initialized = true;

        this.level = level;
        this.tilePos = tilePos;
        this.tilePrefabs = tilePrefabs;
    }

    // update mesh and add an option to update the direct neighbours as well
}
