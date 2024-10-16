using System;
using UnityEngine;

[Serializable]
public struct TileData
{
    public TileType type;                       // what type of tile this is storing
    public Vector2Int pos;                      // the tile position; where this tile resides within the level
    public TowerData towerData;                 // contains the towerdata, is only set if type=TOWER
    [NonSerialized] public MonoTile monoTile;   // we don't need to serialize MonoTile, as it's just an internal utility and it's value serialized is useless to us :3

    public TileData(TileType type, Vector2Int pos, TowerData? towerData = null)
    {
        this.type = type;
        this.pos = pos;
        this.towerData = towerData ?? default;  // use the default keyword instead of null, as nullable variables don't serialize properly in unity
        monoTile = null;                        // this is set by monoTile, when it's instanciated
    }
}
