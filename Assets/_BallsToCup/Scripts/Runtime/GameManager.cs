using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        private GameSettings settings;
        [SerializeField] private PhysicMaterial physicMaterial;
        
        public static int CurrentLevel => GetCurrentLevelSave();

        private void OnEnable()
        {
            settings = GameSettings.Instance;
            physicMaterial.bounciness = settings.ballBounciness;
        }


        private static int GetCurrentLevelSave()
        {
            return PlayerPrefs.GetInt("LevelNo", 0);
        }

        public static void SetCurrentLevelSave(int value)
        {
            PlayerPrefs.SetInt("LevelNo", value);
        }
    }
}