using UnityEngine;
using Framework.Player;
using Framework.Core.StateMachine;

namespace Framework.Animation
{
    public class PlayerAnimationBridge : ActorAnimationBridge
    {
        [Header("Player Reference")] [SerializeField]
        private PlayerController playerController;

        private int _currentActiveStateHash;

        protected override void Awake()
        {
            base.Awake();
            if (playerController == null) playerController = GetComponentInParent<PlayerController>();
        }

        private void OnEnable()
        {
            if (playerController != null)
                playerController.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            if (playerController != null)
                playerController.OnStateChanged -= HandleStateChanged;
        }

        private void Update()
        {
            if (animator == null || motor == null || sensors == null) return;

            float horizontalSpeed = Mathf.Abs(motor.Velocity.x);
            animator.SetFloat(AnimationHashes.Speed, horizontalSpeed);
            animator.SetFloat(AnimationHashes.VerticalVelocity, motor.Velocity.y);
            animator.SetBool(AnimationHashes.IsGrounded, sensors.IsGrounded);
        }

        private void HandleStateChanged(IState newState)
        {
            if (animator == null) return;

            // Clear previously active state animation boolean
            if (_currentActiveStateHash != 0)
            {
                animator.SetBool(_currentActiveStateHash, false);
                _currentActiveStateHash = 0;
            }

            // Set new state animation boolean if registered by the state
            if (newState is BasePlayerState baseState && baseState.AnimationBoolHash != 0)
            {
                _currentActiveStateHash = baseState.AnimationBoolHash;
                animator.SetBool(_currentActiveStateHash, true);
            }
        }

        public void NotifyJumpExecuted()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.JumpTrigger);
        }

        public override void TriggerHurt()
        {
            base.TriggerHurt();
            ClearActiveStateHash();
        }

        public override void TriggerDeath()
        {
            base.TriggerDeath();
            ClearActiveStateHash();
        }

        private void ClearActiveStateHash()
        {
            if (animator != null && _currentActiveStateHash != 0)
            {
                animator.SetBool(_currentActiveStateHash, false);
                _currentActiveStateHash = 0;
            }
        }
    }
}