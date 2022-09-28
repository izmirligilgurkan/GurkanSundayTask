using System;
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
        private Camera mainCam;
        [SerializeField] private Canvas canvas;
        
        private void OnEnable()
        {
            mainCam = Camera.main;
            LevelManager.BallCaptured += OnBallCaptured;
            LevelManager.OnLevelLoaded += OnLevelLoaded;
            //InitializeBallCountText();
        }

        private void OnDisable()
        {
            LevelManager.BallCaptured -= OnBallCaptured;
            LevelManager.OnLevelLoaded -= OnLevelLoaded;
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
            ballCountText.text = $"{LevelManager.CapturedBallCount} / {LevelManager.Instance.currentLevelInstance.level.requiredBallCount}";
        }
        
        
        public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;
 
            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
            {
                return screenPos;
            }
 
            return screenPoint;
        }
    }
}