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

        public void MoveHorizontal(float targetSpeed, float rate)
        {
            float speedDiff = targetSpeed - _rb.linearVelocity.x;
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
    }
}