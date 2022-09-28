using System.Linq;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class LevelInstance : MonoBehaviour
    {
        public bool complete;
        public Level level;
        public GameObject cup;
        private GameObject cupPrefab;
        private GameObject exitTriggerPrefab;
        private int resolution;
        private GameObject tubeBasePrefab;
        private Material tubeMaterial;
        private float tubeRadius;


        public void Construct(Level level, GameObject tubeBasePrefab, GameObject exitTriggerPrefab,
            GameObject cupPrefab, float tubeRadius, Material tubeMaterial, int resolution)
        {
            this.level = level;
            this.tubeBasePrefab = tubeBasePrefab;
            this.exitTriggerPrefab = exitTriggerPrefab;
            this.cupPrefab = cupPrefab;
            this.tubeRadius = tubeRadius;
            this.tubeMaterial = tubeMaterial;
            this.resolution = resolution;
        }

        public void Build()
        {
            TubeController tubeController;
            if (level.useSvg)
            {
                var tubeBase = Instantiate(tubeBasePrefab, transform, true);
                var tubeExtension = CreateTubeExtension();
                tubeExtension.transform.SetParent(tubeBase.transform);
                tubeController = tubeBase.GetComponentInChildren<TubeController>();
            }
            else
            {
                var tube = Instantiate(level.tubePrefabForNoSvg, transform);
                tubeController = tube.GetComponentInChildren<TubeController>();
            }


            SpawnCup();
            SpawnBalls(tubeController);
        }


        private GameObject CreateTubeExtension()
        {
            var controls = SvgMeshUtility.SvgToPoints(level.svgImageForTube);
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

        private void SpawnBalls(TubeController tubeController)
        {
            var col = tubeController.ballZone;
            var center = col.center;
            var radius = col.radius;

            for (var i = 0; i < level.startBallCount; i++)
            {
                var randomPos = (Vector3) Random.insideUnitCircle;
                randomPos *= radius;
                randomPos += col.transform.TransformPoint(center);
                var ball = ObjectPooler.GetPooledObject();

                ball.transform.SetParent(tubeController.transform);
                ball.transform.position = randomPos;
                ball.gameObject.SetActive(true);
            }
        }

        private void SpawnCup()
        {
            cup = Instantiate(cupPrefab, transform);
            cup.transform.position = level.cupPosition;
        }
    }
}