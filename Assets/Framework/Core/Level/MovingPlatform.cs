using UnityEngine;

namespace Framework.Core.Level
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Local Offset Waypoints")]
        [Tooltip("Offsets relative to the platform's starting position")]
        [SerializeField]
        private Vector2[] localOffsets = new Vector2[] { Vector2.zero, new Vector2(4f, 0f) };

        [SerializeField] private float speed = 2.5f;
        [SerializeField] private float waitTimeAtWaypoint = 0.5f;

        private Rigidbody2D _rb;
        private Vector2[] _worldWaypoints;
        private int _currentWaypointIndex;
        private float _waitTimer;
        private bool _isWaiting;

        public Vector2 Velocity { get; private set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            Vector2 origin = transform.position;
            _worldWaypoints = new Vector2[localOffsets.Length];
            for (int i = 0; i < localOffsets.Length; i++)
            {
                _worldWaypoints[i] = origin + localOffsets[i];
            }
        }

        private void FixedUpdate()
        {
            if (_worldWaypoints == null || _worldWaypoints.Length < 2)
            {
                Velocity = Vector2.zero;
                return;
            }

            if (_isWaiting)
            {
                Velocity = Vector2.zero;
                _waitTimer -= Time.fixedDeltaTime;
                if (_waitTimer <= 0f) _isWaiting = false;
                return;
            }

            Vector2 target = _worldWaypoints[_currentWaypointIndex];
            Vector2 currentPosition = _rb.position;
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, target, speed * Time.fixedDeltaTime);

            Velocity = (newPosition - currentPosition) / Time.fixedDeltaTime;

            _rb.MovePosition(newPosition);

            if (Vector2.Distance(newPosition, target) < 0.05f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _worldWaypoints.Length;
                _isWaiting = true;
                _waitTimer = waitTimeAtWaypoint;
                Velocity = Vector2.zero;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (localOffsets == null || localOffsets.Length < 2) return;

            Vector2 origin = Application.isPlaying && _worldWaypoints != null && _worldWaypoints.Length > 0
                ? _worldWaypoints[0] - localOffsets[0]
                : (Vector2)transform.position;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < localOffsets.Length; i++)
            {
                Vector2 current = origin + localOffsets[i];
                Vector2 next = origin + localOffsets[(i + 1) % localOffsets.Length];
                Gizmos.DrawLine(current, next);
                Gizmos.DrawWireSphere(current, 0.2f);
            }
        }
    }
}