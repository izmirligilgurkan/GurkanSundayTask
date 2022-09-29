using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using DG.Tweening;
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
        [SerializeField] private GameObject levelCompletedAnimation;
        [SerializeField] private GameObject levelFailedAnimation;

        private void OnEnable()
        {
            LevelManager.BallCaptured += OnBallCaptured;
            LevelManager.OnLevelLoaded += OnLevelLoaded;
            LevelManager.OnLevelCompleted += OnLevelCompleted;
            LevelManager.OnLevelFailed += OnLevelFailed;
            levelCompletedParent.GetComponentInChildren<Button>().onClick.AddListener(LevelCompletedButtonPressed);
            levelFailedParent.GetComponentInChildren<Button>().onClick.AddListener(LevelFailedButtonPressed);
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
            levelCompletedAnimation.transform.DOScale(Vector3.one * 1.2f, .2f).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InQuad);
        }

        private void OnLevelFailed()
        {
            levelFailedParent.SetActive(true);
            levelFailedAnimation.transform.DOScale(Vector3.one * 1.2f, .2f).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }
        private void LevelCompletedButtonPressed()
        {
            LevelManager.Instance.NextLevel();
            levelCompletedParent.SetActive(false);
        }
        private void LevelFailedButtonPressed()
        {
            LevelManager.Instance.RestartLevel();
            levelFailedParent.SetActive(false);
        }

        private void OnLevelLoaded(Level level)
        {
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
        
    }
}