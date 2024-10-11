using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Enemy")]
public class EnemySpawnData : ScriptableObject
{
    [Header("default values")]
    public Mesh enemyMesh;
    public int baseHealth = 10;
    public float baseSpeed = 1;

    [Header("Level related stats")]
    public int extraHealthPerLevel = 1;
    public float extraSpeedPerLevel = 0.1f;


    public int GetHealth(int level) => baseHealth + (extraHealthPerLevel * level);
    public float GetSpeed(int level) => baseSpeed + (extraSpeedPerLevel * level);
}