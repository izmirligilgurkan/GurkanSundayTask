using System;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime.Patterns
{
  /// <summary>
  /// MonoBehaviour base.
  /// </summary>
  public abstract class BaseMonoBehaviour : MonoBehaviour
  {
    /// <summary>
    /// Transform cached.
    /// </summary>
    public new Transform transform      => cachedTransform ?? (cachedTransform = base.transform);

    /// <summary>
    /// Animation cached.
    /// </summary>
    public new Animation animation      => cachedAnimation ?? (cachedAnimation = this.GetComponent<Animation>());

    /// <summary>
    /// AudioSource cached.
    /// </summary>
    public new AudioSource audio        => cachedAudio ?? (cachedAudio = this.GetComponent<AudioSource>());

    /// <summary>
    /// Collider cached.
    /// </summary>
    public new Collider collider        => cachedCollider ?? (cachedCollider = this.GetComponent<Collider>());

    /// <summary>
    /// RigidBody cached.
    /// </summary>
    public new Rigidbody rigidbody      => cachedRigidbody ?? (cachedRigidbody = this.GetComponent<Rigidbody>());

    /// <summary>
    /// Animator cached.
    /// </summary>
    public Animator animator            => cachedAnimator ?? (cachedAnimator = this.GetComponent<Animator>());

    /// <summary>
    /// Collider2D cached.
    /// </summary>
    public new Collider2D collider2D    => cachedCollider2D ?? (cachedCollider2D = this.GetComponent<Collider2D>());

    /// <summary>
    /// Rigidbody2D cached.
    /// </summary>
    public new Rigidbody2D rigidbody2D  => cachedRigidbody2D ?? (cachedRigidbody2D = this.GetComponent<Rigidbody2D>());

    /// <summary>
    /// RectTransform cached
    /// .</summary>
    public RectTransform rectTransform  => cachedRectTransform ?? (cachedRectTransform = this.GetComponent<RectTransform>());

    [NonSerialized]
    private Transform cachedTransform;

    [NonSerialized]
    private Animation cachedAnimation;

    [NonSerialized]
    private AudioSource cachedAudio;

    [NonSerialized]
    private Animator cachedAnimator;

    [NonSerialized]
    private Collider cachedCollider;

    [NonSerialized]
    private Rigidbody cachedRigidbody;

    [NonSerialized]
    private Collider2D cachedCollider2D;

    [NonSerialized]
    private Rigidbody2D cachedRigidbody2D;

    [NonSerialized]
    private RectTransform cachedRectTransform;

    /// <summary>
    /// Clear all cached components.
    /// </summary>
    public void ClearCachedComponents()
    {
      cachedTransform = null;
      cachedAnimation = null;
      cachedAudio = null;
      cachedAnimator = null;
      cachedCollider = null;
      cachedRigidbody = null;
      cachedCollider2D = null;
      cachedRigidbody2D = null;
      cachedRectTransform = null;
    }
  }
}