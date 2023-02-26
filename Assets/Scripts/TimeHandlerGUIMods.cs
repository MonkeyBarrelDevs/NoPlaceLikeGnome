using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeHandler))]
public class TimeHandlerGUIMods : Editor {

    public override void OnInspectorGUI() {

        TimeHandler timeHandler = GameObject.Find("TimeHandler").GetComponent<TimeHandler>();
        DrawDefaultInspector();
        if (GUILayout.Button("(DEBUG) Advance Day")) {
            timeHandler.SkipDay();
        }
    }
}