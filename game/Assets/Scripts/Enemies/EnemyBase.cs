using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private int level;
    private int health;
    private float speed;

    public bool IsAlive { get; private set; }
    private bool isAlive = false;

    public void Initialize(int level, EnemySpawnData data)
    {
        this.level = level;
        health = data.GetHealth(level);
        speed = data.GetSpeed(level);

        Revive();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    public void Revive()
    {
        gameObject.SetActive(true);
    }
}
