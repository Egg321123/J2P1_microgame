using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] EnemySpawnData spawnData;

    //nodes to move along
    private List<Vector3> nodes;
    private Vector3 randomOffset = Vector3.zero;

    //enemy stats
    private int level;
    private int health;
    private float speed;

    //other
    public bool IsAlive { get; private set; }

    private bool isAlive = false;
    private bool isAllowedToMove = false;

    Coroutine walkRoutine = null;

    private void Start()
    {
        //get the path
        nodes = FindFirstObjectByType<CreateAIPath>().Path;

        //create a random offset to make it neater
        float randomX = Random.Range(-0.25f, 0.25f);
        float randomZ = Random.Range(-0.25f, 0.25f);
        randomOffset = new(randomX, 0, randomZ);
    }

    /// <summary>
    /// run either when instancing this object, or when grabbing it from the pool
    /// </summary>
    /// <param name="level"></param>
    /// <param name="data"></param>
    public void Initialize(int level)
    {
        //sets the values back to needed values
        this.level = level;
        health = spawnData.GetHealth(level);
        speed = spawnData.GetSpeed(level);

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
        isAlive = false;
        isAllowedToMove = false;

        StopCoroutine(walkRoutine);
    }

    public void Revive()
    {
        gameObject.SetActive(true);
        isAlive = true;
        isAllowedToMove = true;

        //start walking
        walkRoutine = StartCoroutine(MoveAlongPath());
    }

    IEnumerator MoveAlongPath()
    {
        //current target node index
        int targetNodeIndex = 1;

        //default targets at start
        Vector3 currentPosition = nodes[0] + randomOffset;
        Vector3 targetPosition = nodes[targetNodeIndex] + randomOffset;

        //iterates through all the pairs of indexes.
        while (targetNodeIndex < nodes.Count)
        {


            //saves data outside of loop
            float deltaTime = 0;
            float distanceAlongLerp = 0f;

            //loop for the current lerp
            while (distanceAlongLerp < 1)
            {
                if (!isAllowedToMove) yield return null;

                //update time
                deltaTime += Time.deltaTime;
                distanceAlongLerp = deltaTime * speed;

                //update current position
                transform.position = Vector3.Lerp(currentPosition, targetPosition, distanceAlongLerp);

                //rotate following the path
                Vector3 dir = (targetPosition - transform.position).normalized;
                if (dir.magnitude != 0) transform.rotation = Quaternion.LookRotation(dir);

                //if reached end, make sure that position is correct, and reset time
                if (distanceAlongLerp >= 1)
                {
                    transform.position = targetPosition;
                    deltaTime = 0;
                }

                yield return null;
            }

            //update the index of the target
            targetNodeIndex++;

            //update target only if it's within the array
            if (targetNodeIndex < nodes.Count)
            {
                //update current position to target current target position, update the new target position
                currentPosition = targetPosition;
                targetPosition = nodes[targetNodeIndex] + randomOffset;
            }

            yield return null;
        }

        yield return null;
    }
}
