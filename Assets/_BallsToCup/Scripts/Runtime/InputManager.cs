using System;
using _BallsToCup.Scripts.Runtime.Patterns;
using Lean.Touch;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class InputManager : BaseMonoBehaviour
    {
        public static event Action<float> OnRotateCommand;

        public void OnFingerUpdate(LeanFinger leanFinger)
        {
            var delta = leanFinger.ScaledDelta.x;
            OnRotateCommand?.Invoke(delta);
        }
    }
}