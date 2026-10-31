using UnityEngine;
using Framework.Enemy;

namespace Extensions
{
    [RequireComponent(typeof(BaseEnemy))]
    public class EnemyAttack : MonoBehaviour
    {
        [Header("Attack Settings")] [SerializeField]
        private float attackRange = 1.3f;

        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private Vector2 knockbackToPlayer = new Vector2(10f, 6f);
        [SerializeField] private LayerMask playerLayer;

        private BaseEnemy _enemy;
        private Animator _animator;
        private Transform _player;
        private float _cooldownTimer;
        private bool _isAttacking;

        private static readonly int AttackTrigger = Animator.StringToHash("Attack");

        public bool IsAttacking => _isAttacking;
        public float AttackRange => attackRange;

        private void Awake()
        {
            _enemy = GetComponent<BaseEnemy>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;
        }

        public bool CanAttack()
        {
            if (_isAttacking || _cooldownTimer > 0f || _player == null) return false;
            return Vector2.Distance(transform.position, _player.position) <= attackRange;
        }

        public void ExecuteAttack()
        {
            if (_isAttacking) return;
            _isAttacking = true;
            _cooldownTimer = attackCooldown;

            _enemy.Motor.SetVelocityX(0f);

            if (_animator != null)
            {
                _animator.SetTrigger(AttackTrigger);
            }
        }

        public void OnAttackHitPoint()
        {
            Collider2D playerHit = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
            if (playerHit != null)
            {
                var deathReceiver = playerHit.GetComponentInParent<PlayerDeathReceiver>();
                if (deathReceiver != null && !deathReceiver.IsDead)
                {
                    float hitDir = Mathf.Sign(playerHit.transform.position.x - transform.position.x);
                    Vector2 impulse = new Vector2(knockbackToPlayer.x * hitDir, knockbackToPlayer.y);
                    deathReceiver.KillAndKnockback(impulse);
                }
            }
        }

        public void OnAttackFinished()
        {
            _isAttacking = false;

            _enemy.Motor.SetVelocityX(_enemy.FacingDirection * _enemy.MoveSpeed);

            var provider = _enemy.GetComponent<EnemyBehaviourProvider>();
            if (provider != null)
            {
                _enemy.SetBehaviour(provider.CreateBehaviour(_enemy));
            }
            else
            {
                _enemy.SetBehaviour(new PatrolBehaviour());
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}