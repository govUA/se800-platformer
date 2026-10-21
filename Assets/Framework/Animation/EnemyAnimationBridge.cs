using UnityEngine;
using Framework.Enemy;

namespace Framework.Animation
{
    public class EnemyAnimationBridge : ActorAnimationBridge
    {
        [Header("Enemy Reference")] [SerializeField]
        private BaseEnemy enemy;

        private int _currentBehaviourHash;

        protected override void Awake()
        {
            base.Awake();
            if (enemy == null) enemy = GetComponentInParent<BaseEnemy>();
        }

        private void OnEnable()
        {
            if (enemy != null)
                enemy.OnBehaviourChanged += HandleBehaviourChanged;
        }

        private void OnDisable()
        {
            if (enemy != null)
                enemy.OnBehaviourChanged -= HandleBehaviourChanged;
        }

        private void Update()
        {
            if (animator == null || motor == null || sensors == null) return;

            float horizontalSpeed = Mathf.Abs(motor.Velocity.x);
            animator.SetFloat(AnimationHashes.Speed, horizontalSpeed);
            animator.SetBool(AnimationHashes.IsGrounded, sensors.IsGrounded);
        }

        private void HandleBehaviourChanged(IEnemyBehaviour newBehaviour)
        {
            if (animator == null) return;

            // Clear previous behavior animator parameter
            if (_currentBehaviourHash != 0)
            {
                animator.SetBool(_currentBehaviourHash, false);
                _currentBehaviourHash = 0;
            }

            // Set new behavior parameter if declared
            if (newBehaviour != null && newBehaviour.AnimationBoolHash != 0)
            {
                _currentBehaviourHash = newBehaviour.AnimationBoolHash;
                animator.SetBool(_currentBehaviourHash, true);
            }
        }
    }
}