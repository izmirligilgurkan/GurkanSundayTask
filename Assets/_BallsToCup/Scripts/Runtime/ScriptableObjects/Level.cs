﻿using Unity.VectorGraphics;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 0)]
    public class Level : ScriptableObject
    {
        public SVGImage svgImageForTube;
        public int startBallCount;
        public int requiredBallCount;
    }
}