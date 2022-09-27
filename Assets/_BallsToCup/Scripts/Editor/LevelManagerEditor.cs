using _BallsToCup.Scripts.Runtime;
using UnityEditor;
using UnityEngine;

namespace _BallsToCup.Scripts.Editor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelManager script = (LevelManager)target;
            if (GUILayout.Button("Create level from values"))
                script.LoadLevelManual();
        }
    }
}

