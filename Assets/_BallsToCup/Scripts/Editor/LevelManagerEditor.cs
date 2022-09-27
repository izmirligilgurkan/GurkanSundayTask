﻿using _BallsToCup.Scripts.Runtime;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace _BallsToCup.Scripts.Editor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LevelManager script = (LevelManager)target;

            CreateEditor(script.manualLoadLevel);
            
            if (GUILayout.Button("Create level manually"))
                script.LoadLevelManual();
            
            DrawDefaultInspector();
            

        }
    }
}

