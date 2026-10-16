using UnityEngine;
using Framework.Player;
using Framework.Player.States;
using Framework.Core.StateMachine;
using Extensions;

namespace Framework.Animation
{
    public class PlayerAnimationBridge : ActorAnimationBridge
    {
        [Header("Player Reference")] [SerializeField]
        private PlayerController playerController;

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

            animator.SetBool(AnimationHashes.IsWallSliding, newState is WallSlideState);
            animator.SetBool(AnimationHashes.IsDashing, newState is DashState);
        }

        // Public API hook for DoubleJumpAbility or Grounded Jump triggers
        public void NotifyJumpExecuted()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.JumpTrigger);
        }

        public override void TriggerHurt()
        {
            base.TriggerHurt();
            // Reset dash flag immediately to avoid visual stuck loops if hurt mid-dash
            if (animator != null) animator.SetBool(AnimationHashes.IsDashing, false);
        }

        public override void TriggerDeath()
        {
            base.TriggerDeath();
            if (animator != null)
            {
                animator.SetBool(AnimationHashes.IsDashing, false);
                animator.SetBool(AnimationHashes.IsWallSliding, false);
            }
        }
    }
}