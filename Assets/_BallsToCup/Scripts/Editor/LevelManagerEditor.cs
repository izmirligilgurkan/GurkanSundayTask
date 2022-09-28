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
            var script = (LevelManager) target;

            CreateEditor(script.manualLoadLevel);

            if (GUILayout.Button("Create level manually"))
                script.LoadLevelManual();

            DrawDefaultInspector();
        }
    }
}