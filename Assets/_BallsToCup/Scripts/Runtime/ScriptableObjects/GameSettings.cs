﻿using UnityEngine;

namespace _BallsToCup.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObjectSingleton<GameSettings>
    {
        public float sensitivity;
    }
}