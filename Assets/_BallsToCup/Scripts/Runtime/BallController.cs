using _BallsToCup.Scripts.Runtime.Patterns;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class BallController : BaseMonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                transform.SetParent(null, true);
            }
        }
    }
}
