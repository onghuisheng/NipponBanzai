using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class CinematicPathNode : MonoBehaviour
{

}


#if UNITY_EDITOR
// Overrides the inspector for CinematicPathing
[CustomEditor(typeof(CinematicPathNode))]
public class CinematicPathNodeEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CinematicPathNode currentObject = (CinematicPathNode)target;

        if (currentObject.transform.parent == null || currentObject.transform.parent.GetComponent<CinematicPathing>() == null)
        {
            DestroyImmediate(currentObject.gameObject);
            Debug.LogError("Error: CinematicPathNode needs to be a child of CinematicPathing");
        }

        if (GUILayout.Button("Add Path"))
        {
            AddNewPath();
        }
    }

    void AddNewPath()
    {
        CinematicPathNode currentObject = (CinematicPathNode)target;
        GameObject go = new GameObject("Path");

        Undo.RegisterCreatedObjectUndo(go, "Add Path");

        go.transform.position = currentObject.transform.position;
        go.transform.parent = currentObject.transform.parent;
        go.transform.SetSiblingIndex(currentObject.transform.GetSiblingIndex() + 1);
        go.AddComponent<CinematicPathNode>();

        Selection.activeGameObject = go;
    }

}
#endif