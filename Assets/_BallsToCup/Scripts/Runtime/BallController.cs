using _BallsToCup.Scripts.Runtime.Patterns;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using Lean.Common;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class BallController : BaseMonoBehaviour
    {
        public bool inCup;
        private LeanConstrainToCollider leanConstrainToCollider;
        private MeshRenderer meshRenderer;

        private void OnEnable()
        {
            inCup = false;
            leanConstrainToCollider = GetComponentInChildren<LeanConstrainToCollider>();
            leanConstrainToCollider.Collider = null;
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color =
                GameSettings.Instance.ballColors[Random.Range(0, GameSettings.Instance.ballColors.Count)];
            LevelManager.CurrentlyActiveBalls.Add(this);
        }

        private void OnDisable()
        {
            LevelManager.CurrentlyActiveBalls.Remove(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7) transform.SetParent(null, true);

            if (other.gameObject.layer == 8 && !inCup)
            {
                leanConstrainToCollider.Collider = other;
                inCup = true;
                LevelManager.BallCaptured?.Invoke();
            }

            if (other.gameObject.layer == 9) LevelManager.BallDestroyed?.Invoke(this);
        }
    }
}