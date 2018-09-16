using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class CinematicPathing : MonoBehaviour
{
    [SerializeField]
    private string m_PathName = "YourPathName";
    /// <summary>
    /// Name of this Pathing
    /// </summary>
    public string pathName {
        get { return m_PathName; }
    }

    [SerializeField]
    private Transform m_LookAtTarget = null;
    /// <summary>
    /// Look at this transform while animating
    /// </summary>
    public Transform lookAtTarget {
        get { return m_LookAtTarget; }
    }

    [SerializeField]
    private float m_MoveSpeed = 10;
    public float moveSpeed {
        get { return m_MoveSpeed; }
    }

    [SerializeField]
    private Ease m_Easing = Ease.Linear;
    public Ease easeType {
        get { return m_Easing; }
    }

    [SerializeField]
    PathType m_PathType = PathType.CatmullRom;
    public PathType pathType {
        get { return m_PathType; }
    }

    [SerializeField]
    bool m_LoopBackToInitial = true;
    /// <summary>
    /// Moves the Camera to where it first begin
    /// </summary>
    public bool loopbackToInitial {
        get { return m_LoopBackToInitial; }
    }


    [MenuItem("GameObject/Cinematic Camera/Camera Pathing", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Cinematic Pathing");
        CinematicPathing pathing = go.AddComponent<CinematicPathing>();

        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    public Transform GetFinalPath()
    {
        if (transform.childCount == 0)
            return null;

        return transform.GetChild(transform.childCount - 1);
    }

    public List<CinematicPathNode> GetPathNodes()
    {
        List<CinematicPathNode> nodes = new List<CinematicPathNode>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            nodes.Add(transform.GetChild(i).GetComponent<CinematicPathNode>());
        }

        return nodes;
    }

    public List<Vector3> GetPathPositions()
    {
        List<Vector3> paths = new List<Vector3>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            paths.Add(transform.GetChild(i).position);
        }

        return paths;
    }

    public static CinematicPathing GetPathWithName(string pathName)
    {
        var paths = FindObjectsOfType<CinematicPathing>();

        foreach (CinematicPathing path in paths)
        {
            if (path.pathName == pathName)
                return path;
        }

        return null;
    }

    public void DoCinematicPath(System.Action onComplete = null, System.Action<int> onWayPointChange = null, System.Action onUpdate = null)
    {
        CameraHandler.GetInstance().DoCinematicPath(this, onComplete, onWayPointChange, onUpdate);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);

            // Destroy child if it doesnt not contain a node
            if (child.GetComponent<CinematicPathNode>() == null)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            if ((i + 1) >= transform.childCount)
                return;

            Handles.DrawLine(child.position, transform.GetChild(i + 1).position);
        }
    }

}

// Overrides the inspector for CinematicPathing
[CustomEditor(typeof(CinematicPathing))]
public class CinematicPathingEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Add Path"))
        {
            AddNewPath();
        }
    }

    void AddNewPath()
    {
        CinematicPathing currentObject = (CinematicPathing)target;
        GameObject go = new GameObject("Path");

        Undo.RegisterCreatedObjectUndo(go, "Add Path");

        if (currentObject.GetFinalPath() != null)
            go.transform.position = currentObject.GetFinalPath().position;
        else
            go.transform.position = currentObject.transform.position;

        go.transform.parent = currentObject.transform;
        go.AddComponent<CinematicPathNode>();

        Selection.activeGameObject = go;
    }

}
