using Unity.VectorGraphics;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 0)]
    public class Level : ScriptableObject
    {
        public bool useSvg;
        public Sprite svgImageForTube;
        public GameObject tubePrefabForNoSvg;
        [Space] 
        public Vector2 cupPosition;
        public int startBallCount;
        public int requiredBallCount;
    }
}