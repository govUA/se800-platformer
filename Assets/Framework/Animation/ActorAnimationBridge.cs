using UnityEngine;
using Framework.Core.Motor;
using Framework.Core.Sensors;

namespace Framework.Animation
{
    [RequireComponent(typeof(Animator))]
    public abstract class ActorAnimationBridge : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        protected Animator animator;

        [SerializeField] protected ActorMotor2D motor;
        [SerializeField] protected EnvironmentDetector2D sensors;

        protected virtual void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (motor == null) motor = GetComponentInParent<ActorMotor2D>();
            if (sensors == null) sensors = GetComponentInParent<EnvironmentDetector2D>();
        }

        public virtual void TriggerHurt()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.Hurt);
        }

        public virtual void TriggerDeath()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.Death);
        }

        public virtual void TriggerAction()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.Action);
        }
    }
}