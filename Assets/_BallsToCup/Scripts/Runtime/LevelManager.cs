using System;
using System.Collections.Generic;
using System.Linq;
using _BallsToCup.Scripts.Runtime.Patterns;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        public static List<BallController> CurrentlyActiveBalls = new List<BallController>();

        public static Action BallCaptured;

        public static Action<BallController> BallDestroyed;
        [SerializeField] public Level manualLoadLevel;
        [SerializeField] private List<Level> levels;

        [Space] [Header("Prefabs")] [SerializeField]
        private GameObject tubeBasePrefab;

        [SerializeField] private GameObject exitTriggerPrefab;
        [SerializeField] private GameObject cupPrefab;

        [Space] [Header("Tube Properties")] [SerializeField]
        private float tubeRadius = .1f;

        [SerializeField] private Material tubeMaterial;
        [SerializeField] private int resolution;


        [HideInInspector] [SerializeField] public LevelInstance currentLevelInstance;
        public static int CapturedBallCount => CurrentlyActiveBalls.Count(controller => controller.inCup);

        private void Awake()
        {
            if (!currentLevelInstance) LoadLevelRuntime(levels[GameManager.CurrentLevel % levels.Count]);
        }

        private void OnEnable()
        {
            BallCaptured += OnBallCaptured;
            BallDestroyed += OnBallDestroyed;
        }

        private void OnDisable()
        {
            BallCaptured -= OnBallCaptured;
            BallDestroyed -= OnBallDestroyed;
        }


        private void OnDrawGizmos()
        {
            if (manualLoadLevel.useSvg)
            {
                //Debug draw for svg tube
                var points = SvgMeshUtility.SvgToPoints(manualLoadLevel.svgImageForTube);
                for (var i = 0; i < points.Length; i++)
                {
                    if (i == points.Length - 1) continue;
                    Gizmos.DrawLine(points[i], points[i + 1]);
                }
            }
        }

        public static event Action<Level> OnLevelLoadStart;
        public static event Action<Level> OnLevelLoaded;

        private void OnBallDestroyed(BallController ball)
        {
            ball.gameObject.SetActive(false);
            if (CurrentlyActiveBalls.All(controller => controller.inCup) &&
                CapturedBallCount < currentLevelInstance.level.requiredBallCount) RestartLevel();
        }

        private void OnBallCaptured()
        {
            if (CapturedBallCount >= currentLevelInstance.level.requiredBallCount && !currentLevelInstance.complete)
            {
                currentLevelInstance.complete = true;
                NextLevel();
            }
        }

        private void NextLevel()
        {
            Destroy(currentLevelInstance.gameObject);
            GameManager.SetCurrentLevelSave(GameManager.CurrentLevel + 1);
            LoadLevelRuntime(levels[GameManager.CurrentLevel % levels.Count]);
        }

        private void RestartLevel()
        {
            Destroy(currentLevelInstance.gameObject);
            LoadLevelRuntime(levels[GameManager.CurrentLevel % levels.Count]);
        }


        public void LoadLevelManual()
        {
            OnLevelLoadStart?.Invoke(manualLoadLevel);

            if (currentLevelInstance) DestroyImmediate(currentLevelInstance.gameObject);

            var levelGameObject = new GameObject("Level");
            var levelInstance = levelGameObject.AddComponent<LevelInstance>();
            levelInstance.Construct(manualLoadLevel, tubeBasePrefab, exitTriggerPrefab, cupPrefab, tubeRadius,
                tubeMaterial, resolution);
            levelInstance.Build();

            currentLevelInstance = levelInstance;

            OnLevelLoaded?.Invoke(manualLoadLevel);
        }

        private void LoadLevelRuntime(Level level)
        {
            OnLevelLoadStart?.Invoke(level);

            var levelGameObject = new GameObject("Level");
            var levelInstance = levelGameObject.AddComponent<LevelInstance>();
            levelInstance.Construct(level, tubeBasePrefab, exitTriggerPrefab, cupPrefab, tubeRadius, tubeMaterial,
                resolution);
            levelInstance.Build();

            currentLevelInstance = levelInstance;

            OnLevelLoaded?.Invoke(level);
        }
    }
}