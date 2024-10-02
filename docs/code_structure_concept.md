# code structure concept
## TileData
- type `TileType`
- position `Vector2Int`
- towerData `Tower?`

## TileType
- EMPTY
- PATH
- TOWER

## Tile : MonoBehaviour
- tileData `ref TileData`

## Game : MonoBehaviour
- lives `int`
- level `LevelData`
- enemies `List<Enemy>`

## Level : MonoBehaviour
- tiles `TileData?[,]`
- money `int`
- Generate() `void`
- SetTile() `void`

## Enemy : MonoBehaviour
- speed `float`
- Move() `void`

## Tower : MonoBehaviour
- towerLevel `int`
- firerate `float`
- Fire() `void`

## Statistics
- enemiesKilled `int`
- towersPlaced `int`
- towerUpgrades `int`
- towerUpgradesTotal `int`
- moneySpent `int`
- moneySpentTotal `int`

## SaveData
- level `int`
- wave `int`
- money `int`
- statistics `Statistics`
