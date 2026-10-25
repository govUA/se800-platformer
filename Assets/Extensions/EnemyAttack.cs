using System.Collections;
using UnityEngine;
using Framework.Enemy;

namespace Extensions
{
    [RequireComponent(typeof(BaseEnemy))]
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Attack Settings")] [SerializeField]
        private float attackRange = 1.3f;

        [SerializeField] private float attackWindup = 0.35f;
        [SerializeField] private float attackCooldown = 1.2f;
        [SerializeField] private Vector2 knockbackToPlayer = new Vector2(10f, 6f);
        [SerializeField] private LayerMask playerLayer;

        private BaseEnemy _enemy;
        private Transform _player;
        private float _cooldownTimer;
        private bool _isAttacking;

        public bool IsAttacking => _isAttacking;
        public float AttackRange => attackRange;

        private void Awake()
        {
            _enemy = GetComponent<BaseEnemy>();
        }

        private void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;
        }

        public bool CanAttack()
        {
            if (_isAttacking || _cooldownTimer > 0f || _player == null) return false;
            return Vector2.Distance(transform.position, _player.position) <= attackRange;
        }

        public void ExecuteAttack()
        {
            if (_isAttacking) return;
            StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            _isAttacking = true;
            _cooldownTimer = attackCooldown;

            // 1. Stop movement during windup
            _enemy.Motor.SetVelocityX(0f);

            // 2. Windup window — player can jump/dash away
            yield return new WaitForSeconds(attackWindup);

            // 3. Check if player is still in reach
            Collider2D playerHit = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
            if (playerHit != null)
            {
                var deathReceiver = playerHit.GetComponentInParent<PlayerDeathReceiver>();
                if (deathReceiver != null)
                {
                    float hitDir = Mathf.Sign(playerHit.transform.position.x - transform.position.x);
                    Vector2 impulse = new Vector2(knockbackToPlayer.x * hitDir, knockbackToPlayer.y);
                    deathReceiver.KillAndKnockback(impulse);
                }
            }

            _isAttacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}