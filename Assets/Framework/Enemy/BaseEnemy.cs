using System;
using UnityEngine;
using Framework.Core.Motor;
using Framework.Core.Sensors;

namespace Framework.Enemy
{
    [RequireComponent(typeof(ActorMotor2D), typeof(EnvironmentDetector2D))]
    public class BaseEnemy : MonoBehaviour
    {
        [Header("Patrol & Edge Settings")] [SerializeField]
        private float moveSpeed = 3f;

        private float _turnCooldownTimer;

        [SerializeField] private Transform ledgeCheckPoint;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float ledgeCheckDistance = 0.5f;
        [SerializeField] private float minTurnInterval = 0.25f; // Prevents flip jitter

        public ActorMotor2D Motor { get; private set; }
        public EnvironmentDetector2D Sensors { get; private set; }
        public float MoveSpeed => moveSpeed;
        public int FacingDirection { get; private set; } = 1;
        public bool CanFlip => _turnCooldownTimer <= 0f;

        public IEnemyBehaviour CurrentBehaviour => _currentBehaviour;
        public event Action<IEnemyBehaviour> OnBehaviourChanged;

        private IEnemyBehaviour _currentBehaviour;

        protected virtual void Awake()
        {
            Motor = GetComponent<ActorMotor2D>();
            Sensors = GetComponent<EnvironmentDetector2D>();
        }

        private void Start()
        {
            if (_currentBehaviour == null)
            {
                var provider = GetComponent<EnemyBehaviourProvider>();
                if (provider != null)
                {
                    SetBehaviour(provider.CreateBehaviour(this));
                }
                else
                {
                    SetBehaviour(new PatrolBehaviour());
                }
            }
        }

        private void Update()
        {
            if (_turnCooldownTimer > 0f) _turnCooldownTimer -= Time.deltaTime;
            _currentBehaviour?.Tick(this);
        }

        private void FixedUpdate()
        {
            _currentBehaviour?.FixedTick(this);
        }

        public void SetBehaviour(IEnemyBehaviour newBehaviour)
        {
            _currentBehaviour?.Exit(this);
            _currentBehaviour = newBehaviour;
            _currentBehaviour?.Enter(this);
            OnBehaviourChanged?.Invoke(_currentBehaviour);
        }

        public void FlipDirection()
        {
            if (_turnCooldownTimer > 0f) return;
            ForceFlipDirection();
        }

        // Forces a flip immediately, ignoring turn cooldowns (e.g. For aiming jumps/attacks).
        public void ForceFlipDirection()
        {
            _turnCooldownTimer = minTurnInterval;
            FacingDirection *= -1;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * FacingDirection;
            transform.localScale = scale;
        }

        public bool IsLedgeAhead()
        {
            // A ledge check is only valid if the enemy is actually grounded
            if (!Sensors.IsGrounded || ledgeCheckPoint == null) return false;
            RaycastHit2D hit =
                Physics2D.Raycast(ledgeCheckPoint.position, Vector2.down, ledgeCheckDistance, groundLayer);
            return hit.collider == null;
        }

        private void OnDrawGizmosSelected()
        {
            if (ledgeCheckPoint != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(ledgeCheckPoint.position, ledgeCheckPoint.position + Vector3.down * ledgeCheckDistance);
            }
        }
    }
}