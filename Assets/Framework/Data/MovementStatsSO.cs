using UnityEngine;

namespace Framework.Data
{
    [CreateAssetMenu(fileName = "MovementStats", menuName = "Platformer Framework/Movement Stats")]
    public class MovementStatsSO : ScriptableObject
    {
        [Header("Horizontal Movement")] [Tooltip("Standard walking top speed")]
        public float walkSpeed = 8f;

        [Tooltip("Max sprint speed")] public float sprintSpeed = 14f;

        [Tooltip("How fast horizontal speed reaches target (m/s^2)")]
        public float acceleration = 65f;

        [Tooltip("Friction when turning or coming to a stop (m/s^2)")]
        public float deceleration = 50f;

        [Tooltip("Multiplier applied to acceleration while in the air (0 = no control, 1 = full control)")]
        [Range(0f, 1f)]
        public float airControlFactor = 0.75f;

        [Header("Jump Physics (Formula Driven)")] [Tooltip("Target peak jump height in units/meters")]
        public float jumpHeight = 4.2f;

        [Tooltip("Time to reach peak jump height (seconds)")]
        public float timeToJumpApex = 0.38f;

        [Tooltip("Gravity multiplier when falling")]
        public float fallGravityMultiplier = 1.6f;

        [Tooltip("Gravity multiplier applied when jump button is released early (variable jump)")]
        public float jumpCutGravityMultiplier = 2.5f;

        [Header("Forgiveness Timings")] [Tooltip("Grace period after leaving a platform where jump is still valid")]
        public float coyoteTime = 0.15f;

        [Tooltip("Input buffer window before hitting the ground")]
        public float jumpBufferTime = 0.12f;

        [Header("Wall Mechanics")] [Tooltip("Maximum slide speed down walls")]
        public float wallSlideSpeed = 3f;

        [Tooltip("Horizontal push force when leaping off a wall")]
        public float wallJumpForce = 12f;

        // Derived physical values calculated dynamically
        public float InitialJumpVelocity => (2f * jumpHeight) / timeToJumpApex;
        public float BaseGravity => (2f * jumpHeight) / Mathf.Pow(timeToJumpApex, 2f);
    }
}