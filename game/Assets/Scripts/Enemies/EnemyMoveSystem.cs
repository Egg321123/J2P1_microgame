using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Jobs;
using System.Collections.Generic;
using System.Linq;

public class EnemyMovementSystem : MonoBehaviour
{
    // this script manages the movement of every enemy, add an enemy to the list if you want it to be controlled by this script,
    // remove it when you want it to stop being controlled by this script.

    private List<EnemyBase> enemies = new();

    // data to pass on to job, native arrays are required when using jobs (multithreading)
    private TransformAccessArray enemyTransforms;
    private NativeArray<Vector3> pathNodes;
    private NativeArray<float> speeds;
    private NativeArray<Vector3> offsets;
    private NativeArray<int> targetNodeIndices;
    private NativeArray<bool> hasReachedEnd;

    // stores a reference to the job itself, to allow completion in late update
    private JobHandle jobHandle;

    // allow enemy to add and remove itself to the movement job
    public void AddEnemy(EnemyBase enemy) => enemies.Add(enemy);
    public void RemoveEnemy(EnemyBase enemy) => enemies.Remove(enemy);

    private void Start()
    {
        // Initialize the path nodes array, this doesn't change (hopefully)
        CreateAIPath pathCreator = FindFirstObjectByType<CreateAIPath>();
        pathNodes = new NativeArray<Vector3>(pathCreator.Path.ToArray(), Allocator.Persistent);
    }

    private void FixedUpdate() { if (enemies.Count != 0) CreateAndRunJob(); }

    private void LateUpdate()
    {
        // Complete the job at the end of the frame
        jobHandle.Complete();

        // update the target indices of the enemies using information from the job
        if (enemies.Count != 0 && targetNodeIndices.IsCreated) 
            for (int i = 0; i < targetNodeIndices.Length; i++) 
                enemies[i].TargetNodeIndex = targetNodeIndices[i];


        // Dispose of data to free up space.
        // (garbage collection doesn't happen automatically on native arrays,
        // you have to dispose of them yourself, otherwise you will get a memory leak.)
        if (enemyTransforms.isCreated) enemyTransforms.Dispose();
        if (speeds.IsCreated) speeds.Dispose();
        if (offsets.IsCreated) offsets.Dispose();
        if (targetNodeIndices.IsCreated) targetNodeIndices.Dispose();

        //for if an enemy has reached the end
        ReachedEnd();
    }

    private void ReachedEnd()
    {
        // check if an enemy has reached the end, if so trigger the proper behavior on those enemies
        if (enemies.Count != 0 && hasReachedEnd.IsCreated)
            for (int i = 0; i < hasReachedEnd.Length; i++)
                if (hasReachedEnd[i]) enemies[i].HasReachedEnd();

        // dispose the native array after checking
        if (hasReachedEnd.IsCreated) hasReachedEnd.Dispose();
    }

    public EnemyMoveJob CreateJobData()
    {
        //update the transform array, this allows a job to modify the tansforms on another thread (job) using some magic
        enemyTransforms = new TransformAccessArray(enemies.Select(e => e.transform).ToArray());

        //create native arrays for all the data we want to pass on to the job, we update this every time due to the list of enemies being able to change
        speeds = new NativeArray<float>(enemies.Select(e => e.Speed).ToArray(), Allocator.Persistent);
        offsets = new NativeArray<Vector3>(enemies.Select(e => e.RandomOffset).ToArray(), Allocator.Persistent);
        targetNodeIndices = new NativeArray<int>(enemies.Select(e => e.TargetNodeIndex).ToArray(), Allocator.Persistent);
        hasReachedEnd = new NativeArray<bool>(enemies.Count, Allocator.Persistent);

        //return the data as an EnemyMoveJob struct, this needs to be filled in fully, otherwise the job gets angry
        return new()
        {
            DeltaTime = Time.deltaTime,
            PathNodes = pathNodes,
            Speeds = speeds,
            Offsets = offsets,
            TargetNodeIndices = targetNodeIndices,
            HasReachedEnd = hasReachedEnd
        };
    }

    public void CreateAndRunJob()
    {
        // creates the job using data created by another method, and schedules (starts/executes) it for our target transforms
        EnemyMoveJob moveJob = CreateJobData();
        jobHandle = moveJob.Schedule(enemyTransforms);
    }

    // dispose of everything when this object is destroyed, otherwise uneccesary memory is used.
    private void OnDestroy()
    {
        // Dispose of data to free up space.
        // (garbage collection doesn't happen automatically on native arrays,
        // you have to dispose of them yourself, otherwise you will get a memory leak.)
        if (enemyTransforms.isCreated) enemyTransforms.Dispose();
        if (speeds.IsCreated) speeds.Dispose();
        if (pathNodes.IsCreated) pathNodes.Dispose();
        if (targetNodeIndices.IsCreated) targetNodeIndices.Dispose();
        if (hasReachedEnd.IsCreated) hasReachedEnd.Dispose();
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
    public NativeArray<bool> HasReachedEnd;

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
        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            //if below the limit of nodes, increment, else reached end
            if (targetNodeIndex + 1 < PathNodes.Length) TargetNodeIndices[index]++;
            else HasReachedEnd[index] = true;
        }
    }
}
