using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(WallHelper))]
public class ToggleWall : Editor
{

    //// Use this for initialization
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WallHelper wall = (WallHelper)target;
        if (GUILayout.Button("Toggle wall"))
        {
            wall.ChangeToggle();
        }
    }
}