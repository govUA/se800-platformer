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

        [SerializeField] private Transform ledgeCheckPoint;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float ledgeCheckDistance = 0.5f;

        public ActorMotor2D Motor { get; private set; }
        public EnvironmentDetector2D Sensors { get; private set; }
        public float MoveSpeed => moveSpeed;
        public int FacingDirection { get; private set; } = 1; // 1 = Right, -1 = Left

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
                SetBehaviour(new PatrolBehaviour());
            }
        }

        private void Update()
        {
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
        }

        public void FlipDirection()
        {
            FacingDirection *= -1;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * FacingDirection;
            transform.localScale = scale;
        }

        public bool IsLedgeAhead()
        {
            if (ledgeCheckPoint == null) return false;
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