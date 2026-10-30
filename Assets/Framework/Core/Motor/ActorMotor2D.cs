using UnityEngine;
using Framework.Data;

namespace Framework.Core.Motor
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ActorMotor2D : MonoBehaviour
    {
        private Rigidbody2D _rb;
        [SerializeField] private MovementStatsSO stats;

        private float _currentGravityMultiplier = 1f;

        public Vector2 Velocity => _rb.linearVelocity;
        public float TargetGravityMultiplier => _currentGravityMultiplier;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f; // Custom gravity is managed entirely via the motor
        }

        private void Start()
        {
            if (stats != null) Initialize(stats);
        }

        public void Initialize(MovementStatsSO movementStats)
        {
            stats = movementStats;
        }

        private void FixedUpdate()
        {
            ApplyCustomGravity();
        }

        private void ApplyCustomGravity()
        {
            if (stats == null) return;

            // Apply: F = m * a where a = baseGravity * multiplier
            float gravityStep = stats.BaseGravity * _currentGravityMultiplier * Time.fixedDeltaTime;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y - gravityStep);
        }

        public void MoveHorizontal(float targetSpeed, float rate, float baseSurfaceSpeed = 0f)
        {
            float desiredSpeed = targetSpeed + baseSurfaceSpeed;
            float speedDiff = desiredSpeed - _rb.linearVelocity.x;
            float movement = speedDiff * Mathf.Min(rate * Time.fixedDeltaTime, 1f);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x + movement, _rb.linearVelocity.y);
        }

        public void ApplyImpulse(Vector2 force)
        {
            _rb.linearVelocity = force;
        }

        public void SetVelocityX(float xVelocity)
        {
            _rb.linearVelocity = new Vector2(xVelocity, _rb.linearVelocity.y);
        }

        public void SetVelocityY(float yVelocity)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, yVelocity);
        }

        public void SetGravityMultiplier(float multiplier)
        {
            _currentGravityMultiplier = multiplier;
        }

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
        }

        public void MoveOnSlope(float targetSpeed, float rate, Vector2 groundNormal, float baseSurfaceSpeed = 0f)
        {
            // Direction parallel to the slope
            Vector2 slopeTangent = new Vector2(groundNormal.y, -groundNormal.x).normalized;

            float desiredSpeed = targetSpeed + baseSurfaceSpeed;

            // Target velocity along the slope
            Vector2 targetVelocity = slopeTangent * desiredSpeed;

            // If on flat ground (or nearly flat), run normal horizontal logic
            if (Mathf.Abs(groundNormal.x) < 0.05f)
            {
                MoveHorizontal(targetSpeed, rate, baseSurfaceSpeed);
                return;
            }

            // On slopes: smoothly interpolate velocity towards the target slope vector
            float step = rate * Time.fixedDeltaTime;
            Vector2 currentVel = _rb.linearVelocity;

            // Adjust velocity towards the slope projection
            float newX = Mathf.MoveTowards(currentVel.x, targetVelocity.x,
                step * Mathf.Abs(targetVelocity.x - currentVel.x + 1f));
            float newY = targetVelocity.y;

            _rb.linearVelocity = new Vector2(newX, newY);
        }
    }
}