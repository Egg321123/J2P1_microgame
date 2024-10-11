using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using System.Collections.Generic;
using System.Linq;

public class EnemyMovementSystem : MonoBehaviour
{
    private List<EnemyBase> enemies = new();

    private NativeArray<Vector3> pathNodes;

    private TransformAccessArray enemyTransforms;
    private NativeArray<Vector3> enemyOffsets;
    private NativeArray<float> enemySpeeds;
    private NativeArray<int> targetNodeIndices;
    private NativeArray<bool> hasReachedEnd;

    private JobHandle moveJobHandle;  // Store the job handle

    public void AddEnemy(EnemyBase enemy) => enemies.Add(enemy);
    public void RemoveEnemy(EnemyBase enemy) => enemies.Remove(enemy);

    private void Start()
    {
        CreateAIPath pathCreator = FindFirstObjectByType<CreateAIPath>();

        // Initialize the path nodes array, this doesn't change (hopefully)
        pathNodes = new NativeArray<Vector3>(pathCreator.Path.ToArray(), Allocator.Persistent);
    }

    private void Update()
    {
        CreateJobData();
        CreateJob();
    }

    private void LateUpdate()
    {
        // Complete the job at the end of the frame
        moveJobHandle.Complete();

        // Check which enemies have reached the end and call the method
        for (int i = 0; i < hasReachedEnd.Length; i++)
        {
            if (hasReachedEnd[i]) enemies[i].HasReachedEnd();
            hasReachedEnd[i] = false;
        }
    }

    public void CreateJobData()
    {
        int enemyCount = enemies.Count;

        // Check if there are no enemies, skip job creation if list is empty
        if (enemyCount == 0)
        {
            // Dispose of data to free up space
            if (enemyTransforms.isCreated) enemyTransforms.Dispose();
            if (enemySpeeds.IsCreated) enemySpeeds.Dispose();
            if (enemyOffsets.IsCreated) enemyOffsets.Dispose();
            if (targetNodeIndices.IsCreated) targetNodeIndices.Dispose();
            if (hasReachedEnd.IsCreated) hasReachedEnd.Dispose(); // Dispose if created

            return;  // Exit the method if no enemies
        }

        // Dispose and create the new arrays based on the current enemies
        if (enemyTransforms.isCreated) enemyTransforms.Dispose();
        enemyTransforms = new TransformAccessArray(enemies.Select(e => e.transform).ToArray());

        if (enemySpeeds.IsCreated) enemySpeeds.Dispose();
        enemySpeeds = new NativeArray<float>(enemies.Select(e => e.Speed).ToArray(), Allocator.Persistent);

        if (enemyOffsets.IsCreated) enemyOffsets.Dispose();
        enemyOffsets = new NativeArray<Vector3>(enemies.Select(e => e.randomOffset).ToArray(), Allocator.Persistent);

        if (hasReachedEnd.IsCreated) hasReachedEnd.Dispose(); // Dispose if created
        hasReachedEnd = new NativeArray<bool>(enemyCount, Allocator.Persistent); // Create new array for end flags

        if (targetNodeIndices.IsCreated)
        {
            for (int i = 0; i < targetNodeIndices.Length; i++)
            {
                if (i < enemyCount - 1) enemies[i].TargetNodeIndex = targetNodeIndices[i];
            }
            targetNodeIndices.Dispose();
        }
        targetNodeIndices = new NativeArray<int>(enemies.Select(e => e.TargetNodeIndex).ToArray(), Allocator.Persistent);
    }

    public void CreateJob()
    {
        // Check if there are any enemies before scheduling the job
        if (enemies.Count == 0)
            return;  // Don't schedule a job if there are no transforms to process

        EnemyMoveJob moveJob = new()
        {
            DeltaTime = Time.deltaTime,
            Speeds = enemySpeeds,
            Offsets = enemyOffsets,
            PathNodes = pathNodes,
            TargetNodeIndices = targetNodeIndices,
            HasReachedEnd = hasReachedEnd
        };

        moveJobHandle = moveJob.Schedule(enemyTransforms);
    }

    //dispose of everything
    private void OnDestroy()
    {
        // Dispose of NativeLists and TransformAccessArray
        if (enemyTransforms.isCreated) enemyTransforms.Dispose();
        if (enemySpeeds.IsCreated) enemySpeeds.Dispose();
        if (pathNodes.IsCreated) pathNodes.Dispose();
        if (targetNodeIndices.IsCreated) targetNodeIndices.Dispose();
    }

    private void OnDrawGizmos()
    {
        if (pathNodes != null) foreach (Vector3 node in pathNodes) Gizmos.DrawSphere(node, 0.1f);
    }
}

[BurstCompile]
public struct EnemyMoveJob : IJobParallelForTransform
{
    [ReadOnly] public float DeltaTime;
    [ReadOnly] public NativeArray<float> Speeds;
    [ReadOnly] public NativeArray<Vector3> Offsets;
    [ReadOnly] public NativeArray<Vector3> PathNodes;
    public NativeArray<int> TargetNodeIndices;
    public NativeArray<bool> HasReachedEnd;

    // Move every object with its own data
    public void Execute(int index, TransformAccess transform)
    {
        // Get the current target node index
        int targetNodeIndex = TargetNodeIndices[index];

        // If target index is 0, position at the beginning of the path and increment the target position
        Vector3 currentPosition;
        if (targetNodeIndex == 0)
        {
            transform.position = PathNodes[targetNodeIndex];
            targetNodeIndex++;
            TargetNodeIndices[index] = targetNodeIndex;  // Update the target node index in array here
            return;
        }
        else currentPosition = transform.position;

        // Get the target position
        Vector3 targetPosition = PathNodes[targetNodeIndex] + Offsets[index];

        // Get the direction of this node
        Vector3 direction = (targetPosition - currentPosition).normalized;

        // Create the amount of distance the object needs to move
        float moveStep = Speeds[index] * DeltaTime;

        // Update the transform based on the distance you need to go
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveStep);

        // Check if needs to be rotated, if so rotate
        if (direction.magnitude > 0) transform.rotation = Quaternion.LookRotation(direction);

        // Check if enemy has reached the target node and increment the index
        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            // Ensure the index doesn't go out of bounds
            targetNodeIndex = Mathf.Min(targetNodeIndex + 1, PathNodes.Length - 1);
            TargetNodeIndices[index] = targetNodeIndex;  // Update the target node index here too

            // Check if this is the last node, if so set flag for end reached
            if (targetNodeIndex >= PathNodes.Length - 1) HasReachedEnd[index] = true;
        }
    }
}
