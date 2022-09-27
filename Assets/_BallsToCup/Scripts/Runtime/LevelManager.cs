using System.Collections.Generic;
using System.Linq;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using Unity.VectorGraphics;
using Unity.VectorGraphics.Editor;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] public Level manualLoadLevel;
        [SerializeField] private List<Level> levels;
        [Space]
        [Header("Prefabs")]
        [SerializeField] private GameObject tubeBasePrefab;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject exitTriggerPrefab;
        [SerializeField] private GameObject cupPrefab;
        
        [Space]
        [Header("Tube Properties")]
        [SerializeField] private float tubeRadius = .1f;
        [SerializeField] private Material tubeMaterial;
        [SerializeField] private int resolution;


        private GameObject currentLevel;
        private void OnDrawGizmos()
        {
            //Debug draw for svg tube
            var points = SvgMeshUtility.SvgToPoints(manualLoadLevel.svgImageForTube, manualLoadLevel.invertShape);
            for (int i = 0; i < points.Length; i++)
            {
                if (i == points.Length - 1) continue;
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }


        public void LoadLevelManual()
        {
            LoadLevelEditor(manualLoadLevel);
        }
        
        private void LoadLevelRuntime(Level level)
        {
            
        }

        private void LoadLevelEditor(Level level)
        {
            if (currentLevel) DestroyImmediate(currentLevel);
            
            var levelInstance = new GameObject("Level");
            var tubeBase = Instantiate(tubeBasePrefab, levelInstance.transform, true);
            var tubeExtension = CreateTubeExtension(level);
            tubeExtension.transform.SetParent(tubeBase.transform);
            SpawnBalls(tubeBase.GetComponentInChildren<TubeController>(), level);
            currentLevel = levelInstance;
        }

        private GameObject CreateTubeExtension(Level level)
        {
            var controls = SvgMeshUtility.SvgToPoints(level.svgImageForTube, level.invertShape);
            var meshes = SvgMeshUtility.CreateTubeMesh(tubeRadius, resolution, controls.ToList());
            var outer = new GameObject("Outer Tube");
            var outerMeshFilter = outer.AddComponent<MeshFilter>();
            var outerMeshRenderer = outer.AddComponent<MeshRenderer>();
            var outerCollider = outer.AddComponent<MeshCollider>();

            var inner = new GameObject("Inner Tube");
            var innerMeshFilter = inner.AddComponent<MeshFilter>();
            var innerMeshRenderer = inner.AddComponent<MeshRenderer>();
            var innerCollider = inner.AddComponent<MeshCollider>();

            outerMeshFilter.mesh = meshes[0];
            innerMeshFilter.mesh = meshes[1];
            outerMeshRenderer.material = tubeMaterial;
            innerMeshRenderer.material = tubeMaterial;
            outerCollider.sharedMesh = meshes[0];
            innerCollider.sharedMesh = meshes[1];

            inner.transform.SetParent(outer.transform, true);

            var lastPoint = controls[controls.Length - 1];
            var secondLastPoint = controls[controls.Length - 2];
            var lastDirection = (lastPoint - secondLastPoint).normalized;
            var exitTriggerInstance = Instantiate(exitTriggerPrefab, outer.transform, true);
            exitTriggerInstance.transform.position = lastPoint;
            exitTriggerInstance.transform.up = lastDirection;

            return outer;
        }

        private void SpawnBalls(TubeController tubeController, Level level)
        {
            var col = tubeController.ballZone;
            var center = col.center;
            var radius = col.radius;
            var volume = Mathf.PI * radius * radius * radius * 4f / 3f;
            var ballVolume = volume / level.startBallCount;
            var ballRadius = Mathf.PI * 4f / (ballVolume * 3f);
            for (int i = 0; i < level.startBallCount; i++)
            {
                var randomPos = (Vector3)Random.insideUnitCircle;
                randomPos *= radius;
                randomPos += col.transform.TransformPoint(center);
                var ball = Instantiate(ballPrefab, tubeController.transform);
                //ball.transform.localScale = Vector3.one * (ballRadius * 2f);
                ball.transform.position = randomPos;
            }

        }

        
    }
}