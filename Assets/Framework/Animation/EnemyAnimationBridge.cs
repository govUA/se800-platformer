using UnityEngine;
using Framework.Enemy;

namespace Framework.Animation
{
    public class EnemyAnimationBridge : ActorAnimationBridge
    {
        [Header("Enemy Reference")] [SerializeField]
        private BaseEnemy enemy;

        private bool _isChasing;

        protected override void Awake()
        {
            base.Awake();
            if (enemy == null) enemy = GetComponentInParent<BaseEnemy>();
        }

        private void Update()
        {
            if (animator == null || motor == null || sensors == null) return;

            float horizontalSpeed = Mathf.Abs(motor.Velocity.x);
            animator.SetFloat(AnimationHashes.Speed, horizontalSpeed);
            animator.SetBool(AnimationHashes.IsGrounded, sensors.IsGrounded);
            animator.SetBool(AnimationHashes.IsChasing, _isChasing);
        }

        public void SetChasing(bool chasing)
        {
            _isChasing = chasing;
        }
    }
}