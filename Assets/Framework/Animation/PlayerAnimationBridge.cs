using UnityEngine;
using Framework.Player;
using Framework.Player.States;
using Framework.Core.StateMachine;

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
        }
    }
}