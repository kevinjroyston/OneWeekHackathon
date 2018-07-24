using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(MazeHandler))]
public class BuildMaze : Editor
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MazeHandler mazeScript = (MazeHandler)target;
        if (GUILayout.Button("Resize Maze Grid"))
        {
            mazeScript.ClearMaze();
            mazeScript.BuildMaze();
        }
    }
}