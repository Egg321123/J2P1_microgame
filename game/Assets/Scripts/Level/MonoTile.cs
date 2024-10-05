using System;
using UnityEngine;

public class MonoTile : MonoBehaviour
{
    private Level level = null;
    private Vector2Int tilePos = Vector2Int.one * -1;

    public TileData Data => level.GetTile(tilePos);

    /// <summary>
    /// sets the level, throws <see cref="InvalidOperationException"/> when already set
    /// </summary>
    public void SetLevel(Level level)
    {
        if (this.level != null) throw new InvalidOperationException("attempted to set level whilst level was already set!");
        this.level = level;
    }

    /// <summary>
    /// sets the tile position, throws <see cref="InvalidOperationException"/> when already set
    /// </summary>
    public void SetTilePos(Vector2Int tilePos)
    {
        if (this.tilePos != (Vector2Int.one * -1)) throw new InvalidOperationException("attempted to set the tile position whilst the tile position was already set");
        this.tilePos = tilePos;
    }
}
