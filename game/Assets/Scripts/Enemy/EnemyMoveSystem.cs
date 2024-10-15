using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using System.Collections.Generic;

public class EnemyMovementSystem : MonoBehaviour
{
    // this script manages the movement of every enemy, add an enemy to the list if you want it to be controlled by this script,
    // remove it when you want it to stop being controlled by this script.

    //enemies hold all enemies, enemiesToUpdate is a list of all enemies used in the current job, to avoid shifting list issues
    private List<EnemyBase> enemies = new();
    private List<EnemyBase> enemiesToUpdate = new();

    //store some values to avoid having to call pathcreator more then needed
    public Vector3 PathStart { get; private set; }
    private int pathReachedEndIndex;

    // data to pass on to job, native arrays are required when using jobs (multithreading)
    private TransformAccessArray enemyTransforms;
    private NativeArray<Vector3> pathNodes;
    private NativeArray<float> speeds;
    private NativeArray<Vector3> offsets;
    private NativeArray<int> targetNodeIndices;
    private CreateAIPath pathCreator;
    // stores a reference to the job itself
    private JobHandle jobHandle;

    // allow enemy to add and remove itself to the movement job
    public void AddEnemy(EnemyBase enemy) => enemies.Add(enemy);
    public void RemoveEnemy(EnemyBase enemy) => enemies.Remove(enemy);

    private void Start()
    {
        pathCreator = FindFirstObjectByType<CreateAIPath>();
        pathCreator.RegeneratedPaths += InitializePathNodes;    // add the method as listener to the regenerated paths event
        InitializePathNodes();                                  // initialize the path nodes, because in Start() the path has already been generated
    }

    public void InitializePathNodes()
    {
        PathStart = pathCreator.Path[0];
        pathReachedEndIndex = pathCreator.Path.Count;

        // Displose the pathnodes if it has alweady been cweated >//<
        if (pathNodes.IsCreated)
            pathNodes.Dispose();

        pathNodes = new NativeArray<Vector3>(pathCreator.Path.ToArray(), Allocator.Persistent);
    }

    private void LateUpdate()
    {
        //if the job is completed, complete the job, write some data to the ai and create a new job
        if (jobHandle.IsCompleted)
        {
            // Complete the job at the end of the frame
            jobHandle.Complete();

            //check if the enemy count is higher then 0, and if all the arrays are made (by checking the main one)
            if (enemiesToUpdate.Count > 0 && enemyTransforms.isCreated)
            {
                // Update target indices of the enemies using information from the job
                for (int i = 0; i < enemiesToUpdate.Count; i++)
                {
                    enemiesToUpdate[i].TargetNodeIndex = targetNodeIndices[i];
                    if (enemiesToUpdate[i].TargetNodeIndex == pathReachedEndIndex) enemiesToUpdate[i].HasReachedEnd();
                }
            }

            // Dispose of the data that changes between jobs to free up space
            DisposeMost();

            // Create and run a new job only if there are enemies present
            if (enemies.Count > 0) jobHandle = CreateJobData().Schedule(enemyTransforms);
        }
    }

    public EnemyMoveJob CreateJobData()
    {
        enemiesToUpdate = new(enemies);

        //create new native arrays with the length of enemies
        enemyTransforms = new TransformAccessArray(enemiesToUpdate.Count);
        speeds = new NativeArray<float>(enemiesToUpdate.Count, Allocator.Persistent);
        offsets = new NativeArray<Vector3>(enemiesToUpdate.Count, Allocator.Persistent);
        targetNodeIndices = new NativeArray<int>(enemiesToUpdate.Count, Allocator.Persistent);

        //fill all the native arrays in a single for loop, skip hasReachedEnd because we fill it with false for every enemy
        for (int i = 0; i < enemiesToUpdate.Count; i++)
        {
            EnemyBase enemy = enemiesToUpdate[i];
            enemyTransforms.Add(enemy.transform);
            speeds[i] = enemy.Speed;
            offsets[i] = enemy.RandomOffset;
            targetNodeIndices[i] = enemy.TargetNodeIndex;
        }

        //return the data as an EnemyMoveJob struct, this needs to be filled in fully, otherwise the job gets angry
        return new()
        {
            DeltaTime = Time.deltaTime,
            PathNodes = pathNodes,
            Speeds = speeds,
            Offsets = offsets,
            TargetNodeIndices = targetNodeIndices,
        };
    }


    // dispose of everything when this object is destroyed, otherwise uneccesary memory is used.
    private void OnDestroy()
    {
        jobHandle.Complete();
        DisposeMost();
        if (pathNodes.IsCreated) pathNodes.Dispose();
    }

    /// <summary>
    /// Dispose of data that changes between jobs to free up space.
    /// (garbage collection doesn't happen automatically on native arrays,
    /// you have to dispose of them yourself, otherwise you will get a memory leak.)
    /// </summary>
    private void DisposeMost()
    {
        if (enemyTransforms.isCreated) enemyTransforms.Dispose();
        if (speeds.IsCreated) speeds.Dispose();
        if (offsets.IsCreated) offsets.Dispose();
        if (targetNodeIndices.IsCreated) targetNodeIndices.Dispose();
    }
}

//burst compiler, very efficient, but limited unity functions, also garbage collection needs to be done by yourself
[BurstCompile]
public struct EnemyMoveJob : IJobParallelForTransform
{
    // data as a struct, when you don't want a job to modify data make sure it's set to readonly
    [ReadOnly] public float DeltaTime;
    [ReadOnly] public NativeArray<Vector3> PathNodes;
    [ReadOnly] public NativeArray<float> Speeds;
    [ReadOnly] public NativeArray<Vector3> Offsets;
    public NativeArray<int> TargetNodeIndices;

    // Move every object with its own data
    public void Execute(int index, TransformAccess transform)
    {
        // Get the information for this specific index
        float speed = Speeds[index];
        Vector3 offset = Offsets[index];
        int targetNodeIndex = TargetNodeIndices[index];

        // If target index is 0 (at the start of the path), position at the beginning and increment the target position
        if (targetNodeIndex == 0)
        {
            transform.position = PathNodes[targetNodeIndex];
            transform.rotation = Quaternion.identity;
            targetNodeIndex++;
            TargetNodeIndices[index] = targetNodeIndex;
        }

        // save the current and target position
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = PathNodes[targetNodeIndex] + offset;

        // Update the transform based on the distance you need to go
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * DeltaTime);

        // Check if needs to be rotated, if so rotate
        Vector3 direction = (targetPosition - currentPosition).normalized;
        if (direction.magnitude > 0) transform.rotation = Quaternion.LookRotation(direction);

        // Check if enemy has reached the target node with a tolerance
        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f) TargetNodeIndices[index]++;
    }
}
