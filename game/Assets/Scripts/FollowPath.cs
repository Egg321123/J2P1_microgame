using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool isAllowedToMove = true;
    [SerializeField] private bool hasReachedEnd = false;

    private List<Vector3> nodes;
    private Vector3 randomOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartWalking();
    }

    public void StartWalking()
    {
        IsAllowedToMove = true;
        hasReachedEnd = false;

        if (nodes == null) StartCoroutine(WaitForPath());
        else StartCoroutine(MoveAlongPath());
    }

    // Coroutine that waits for the path to be initialized
    private IEnumerator WaitForPath()
    {
        CreateAIPath pathCreator = FindFirstObjectByType<CreateAIPath>();

        // Keep checking until the path from CreateAIPath is initialized
        while (pathCreator.Path == null) yield return null;

        // Now the path is no longer null, we can proceed
        nodes = pathCreator.Path;

        // Create a random offset
        System.Random random = new();
        float randomX = (float)random.NextDouble() - 0.5f;
        float randomZ = (float)random.NextDouble() - 0.5f;
        randomOffset = new(randomX / 1.5f, 0, randomZ / 1.5f);

        //TEMPORARY, REMOVE LATER
        RandomSpeed();

        // Now start moving along the path
        StartCoroutine(MoveAlongPath());
        yield return null;
    }

    /// <summary>
    /// REMOVE LATER, JUST FOR TESTING
    /// </summary>
    private void RandomSpeed()
    {
        speed = Random.Range(1,5);
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
                //update time
                if (isAllowedToMove) deltaTime += Time.deltaTime;
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

        hasReachedEnd = true;
        HasReachedEndBehavior();

        yield return null;
    }

    public bool IsAllowedToMove { set { isAllowedToMove = value; } }
    public bool HasReachedEnd { get { return hasReachedEnd; } }

    private void HasReachedEndBehavior()
    {
        //hide the object when it has reached the end (death, but no longer deleting the object, to allow pooling)
        gameObject.SetActive(false);
    }
}
