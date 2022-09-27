using _BallsToCup.Scripts.Runtime.Patterns;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class TubeController : BaseMonoBehaviour
    {
        private float rotateValue;
        private GameSettings gameSettings;
        private float Sensitivity => gameSettings.sensitivity;
        [SerializeField] public SphereCollider ballZone;
        
        private void OnEnable()
        {
            gameSettings = GameSettings.Instance;
            InputManager.OnRotateCommand += OnRotateCommand;
        }

        private void OnDisable()
        {
            InputManager.OnRotateCommand -= OnRotateCommand;
        }

        private void OnRotateCommand(float delta)
        {
            rotateValue += delta;
        }

        private void FixedUpdate()
        {
            transform.rotation *= Quaternion.AngleAxis(rotateValue * Sensitivity, Vector3.forward);
            rotateValue = 0;
        }
    }
}
