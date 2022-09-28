using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _BallsToCup.Scripts.Runtime
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI ballCountText;
        [SerializeField] private GameObject levelCompletedParent;
        [SerializeField] private GameObject levelFailedParent;
        [SerializeField] private Canvas canvas;
        private Camera mainCam;

        private void OnEnable()
        {
            mainCam = Camera.main;
            LevelManager.BallCaptured += OnBallCaptured;
            LevelManager.OnLevelLoaded += OnLevelLoaded;
            LevelManager.OnLevelCompleted += OnLevelCompleted;
            LevelManager.OnLevelFailed += OnLevelFailed;
            levelCompletedParent.GetComponentInChildren<Button>().onClick.AddListener(LevelCompletedButtonPressed);
            levelFailedParent.GetComponentInChildren<Button>().onClick.AddListener(LevelFailedButtonPressed);

            //InitializeBallCountText();
        }

        private void OnDisable()
        {
            LevelManager.BallCaptured -= OnBallCaptured;
            LevelManager.OnLevelLoaded -= OnLevelLoaded;
            LevelManager.OnLevelCompleted -= OnLevelCompleted;
            LevelManager.OnLevelFailed -= OnLevelFailed;
        }

        private void OnLevelCompleted()
        {
            levelCompletedParent.SetActive(true);
        }

        private void OnLevelFailed()
        {
            levelFailedParent.SetActive(true);
        }
        private void LevelCompletedButtonPressed()
        {
            LevelManager.Instance.NextLevel();
        }
        private void LevelFailedButtonPressed()
        {
            LevelManager.Instance.RestartLevel();
        }

        private void OnLevelLoaded(Level level)
        {
            levelCompletedParent.SetActive(false);
            levelFailedParent.SetActive(false);
            InitializeBallCountText();
            SetLevelText();
        }
        

        private void SetLevelText()
        {
            levelText.text = $"LEVEL {GameManager.CurrentLevel + 1}";
        }


        private void InitializeBallCountText()
        {
            ballCountText.text = $"0 / {LevelManager.Instance.currentLevelInstance.level.requiredBallCount}";
        }

        private void OnBallCaptured()
        {
            ballCountText.text =
                $"{LevelManager.CapturedBallCount} / {LevelManager.Instance.currentLevelInstance.level.requiredBallCount}";
        }


        public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
        {
            var screenPoint = cam.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;

            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
                return screenPos;

            return screenPoint;
        }
    }
}