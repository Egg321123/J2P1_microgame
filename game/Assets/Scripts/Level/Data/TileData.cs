using System;
using UnityEngine;

[Serializable]
public struct TileData
{
    public TileType type;
    public Vector2Int pos;
    public TowerData towerData;
    [NonSerialized] public MonoTile monoTile;

    public TileData(TileType type, Vector2Int pos, TowerData? towerData = null)
    {
        this.type = type;
        this.pos = pos;
        this.towerData = towerData ?? default;
        monoTile = null;
    }
}
