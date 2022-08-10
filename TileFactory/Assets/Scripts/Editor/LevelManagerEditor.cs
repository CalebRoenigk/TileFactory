using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LevelManager script = (LevelManager) target;

            if (GUILayout.Button("Save Level"))
            {
                script.SaveMap();
            }
            
            if (GUILayout.Button("Clear Tilemaps"))
            {
                script.ClearMap();
            }
            
            if (GUILayout.Button("Load Level"))
            {
                script.LoadMap();
            }
        }
    }
}

