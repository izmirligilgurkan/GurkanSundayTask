using System;
using System.Collections.Generic;
using System.Linq;
using _BallsToCup.Scripts.Runtime.Patterns;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private Material backgroundMaterial;
        

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
        public static event Action OnLevelFailed;
        public static event Action OnLevelCompleted;

        private void OnBallDestroyed(BallController ball)
        {
            ball.transform.SetParent(null);
            ball.gameObject.SetActive(false);
            if (CurrentlyActiveBalls.All(controller => controller.inCup) &&
                CapturedBallCount < currentLevelInstance.level.requiredBallCount) OnLevelFailed?.Invoke();
        }

        private void OnBallCaptured()
        {
            if (CapturedBallCount >= currentLevelInstance.level.requiredBallCount && !currentLevelInstance.complete)
            {
                currentLevelInstance.complete = true;
                OnLevelCompleted?.Invoke();
            }
            else if (CurrentlyActiveBalls.All(controller => controller.inCup) &&
                CapturedBallCount < currentLevelInstance.level.requiredBallCount) OnLevelFailed?.Invoke();
        }

        public void NextLevel()
        {
            currentLevelInstance.transform.DOMoveX(-10f, .5f).SetRelative().SetEase(Ease.InBack).OnComplete(AfterAnimation);
            
            void AfterAnimation()
            {
                Destroy(currentLevelInstance.gameObject);
                GameManager.SetCurrentLevelSave(GameManager.CurrentLevel + 1);
                LoadLevelRuntime(levels[GameManager.CurrentLevel % levels.Count]);
            }
        }

        

        public void RestartLevel()
        {
            Destroy(currentLevelInstance.gameObject);
            LoadLevelRuntime(levels[GameManager.CurrentLevel % levels.Count]);
        }

        private void LevelLoadStart(Level level)
        {
            OnLevelLoadStart?.Invoke(level);
            
        }

        private void LevelLoadEnd(Level level)
        {
            OnLevelLoaded?.Invoke(level);
        }


        public void LoadLevelManual()
        {
            LevelLoadStart(manualLoadLevel);
            
            if (currentLevelInstance) DestroyImmediate(currentLevelInstance.gameObject);

            LoadLevel(manualLoadLevel);

            LevelLoadEnd(manualLoadLevel);
        }

        private void LoadLevelRuntime(Level level)
        {
            LevelLoadStart(level);

            LoadLevel(level);

            LevelLoadEnd(level);
        }

        private void LoadLevel(Level level)
        {
            Random.InitState(GameManager.CurrentLevel);
            var randomColor = GameSettings.Instance.ballColors[Random.Range(0, GameSettings.Instance.ballColors.Count)];
            var mixedColor = Color.Lerp(randomColor, Color.white, .3f);
            backgroundMaterial.color = mixedColor;
            var levelGameObject = new GameObject("Level");
            var levelInstance = levelGameObject.AddComponent<LevelInstance>();
            levelInstance.Construct(level, tubeBasePrefab, exitTriggerPrefab, cupPrefab, tubeRadius, tubeMaterial,
                resolution);
            levelInstance.Build();
            var initPos = levelInstance.transform.position;
            levelInstance.transform.position += Vector3.right * 10f;
            ToggleBallRigidbodies(false);
            levelInstance.transform.DOMove(initPos, .5f).SetEase(Ease.OutBack).SetUpdate(UpdateType.Fixed).OnComplete(() => ToggleBallRigidbodies(true));
            
            currentLevelInstance = levelInstance;
        }

        private void ToggleBallRigidbodies(bool enable)
        {
            foreach (var controller in CurrentlyActiveBalls)
            {
                controller.rigidbody.isKinematic = !enable;
                controller.rigidbody.velocity = Vector3.zero;
            }
        }
    }
}