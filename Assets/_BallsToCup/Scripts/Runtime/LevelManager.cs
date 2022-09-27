using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Level manualLoadLevel;
        
        public void LoadLevelManual()
        {
            LoadLevelEditor(manualLoadLevel); //todo: overload for editor mode
        }

        private void LoadLevelEditor(Level level)
        {
            
        }

        private void LoadLevelRuntime(Level level)
        {
            
        }
    }
}