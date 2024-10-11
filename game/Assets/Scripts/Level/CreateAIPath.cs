using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAIPath : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    //path for ai to use
    public List<Vector3> Path { get; private set; } = null;

    // Start is called before the first frame update
    void Awake()
    {
        RegeneratePath();
    }

    public void RegeneratePath()
    {
        StartCoroutine(WaitForLevel());
    }

    IEnumerator WaitForLevel()
    {
        Level level = null;
        while (level == null) {
            level = FindFirstObjectByType<MonoLevel>().Level;
            yield return null;
        }

        IReadOnlyList<Vector2Int> list = level.GetPath();
        Path = AdjustListToWorld(list, level);
        CreateLineRenderer();

        yield return null;
    }

    private void CreateLineRenderer()
    {
        lineRenderer.positionCount = Path.Count;

        for (int i = 0; i < Path.Count; i++)
        {
            Vector3 offsetPos = Path[i] + new Vector3(0, 0.01f, 0);
            lineRenderer.SetPosition(i, offsetPos);
        }
    }

    // Method to convert a List<Vector2Int> to List<Vector3> with the proper offset
    private List<Vector3> AdjustListToWorld(IReadOnlyList<Vector2Int> posList, Level level)
    {
        List<Vector3> worldPosList = new(posList.Count);
        foreach (Vector2Int pos in posList)
        {
            worldPosList.Add(level.GetTile(pos).monoTile.transform.position);
        }
        return worldPosList;
    }

#if UNITY_EDITOR
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        if (Path != null)
        {
            if (Path.Count != 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(Path[0], 0.5f);

                //creates a visible path for editor
                Gizmos.color = Color.white;
                Gizmos.DrawLineStrip(Path.ToArray(), false);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Path[^1], 0.5f);
            }
        }
    }
#endif
}
