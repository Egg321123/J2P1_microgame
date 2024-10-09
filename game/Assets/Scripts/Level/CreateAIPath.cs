using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAIPath : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    //path for ai to use
    public List<Vector3> Path { get; private set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        RegeneratePath();
    }

    public void RegeneratePath()
    {
        //get the list of coords and convert it to a path the ai can use (centered on the tiles)
        List<Vector2Int> list = new(FindFirstObjectByType<MonoLevel>().Level.GetPath());
        Path = AdjustListToWorld(list);
        CreateLineRenderer();
    }

    private void CreateLineRenderer()
    {
        lineRenderer.positionCount = Path.Count;

        for (int i = 0; i < Path.Count; i++) 
        {
            Vector3 offsetPos = Path[i] + new Vector3(0,0.01f,0);
            lineRenderer.SetPosition(i, offsetPos);
        }
    }

    // Method to convert a List<Vector2Int> to List<Vector3> with the proper offset
    private List<Vector3> AdjustListToWorld(List<Vector2Int> vector2IntList, float yValue = 0f)
    {
        List<Vector3> vector3List = new();
        foreach (Vector2Int vector2Int in vector2IntList)
        {
            vector3List.Add(new(vector2Int.x + 0.5f, yValue, vector2Int.y + 0.5f));
        }
        return vector3List;
    }

#if UNITY_EDITOR
    // draw path for debuggong
    private void OnDrawGizmos()
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
