using UnityEngine;

namespace Framework.Animation
{
    public static class AnimationHashes
    {
        // Locomotion floats & booleans
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int IsWallSliding = Animator.StringToHash("IsWallSliding");

        // Action & Reaction triggers
        public static readonly int Hurt = Animator.StringToHash("Hurt");
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int Action = Animator.StringToHash("Action");

        // Extension Ability Hooks
        public static readonly int IsDashing = Animator.StringToHash("IsDashing");
        public static readonly int JumpTrigger = Animator.StringToHash("Jump");
        public static readonly int IsChasing = Animator.StringToHash("IsChasing");
    }
}