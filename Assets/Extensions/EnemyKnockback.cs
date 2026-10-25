using System.Collections;
using Framework.Enemy;
using UnityEngine;

namespace Extensions
{
    public class EnemyKnockback : MonoBehaviour
    {
        [SerializeField] private float deathAnimationDuration = 0.75f;

        private Rigidbody2D _rb;
        private BaseEnemy _baseEnemy;
        private Animator _animator;
        private Coroutine _knockbackRoutine;
        private bool _isDying;

        public bool IsInKnockback { get; private set; }
        public bool IsDying => _isDying;

        private static readonly int HurtHash = Animator.StringToHash("Hurt");
        private static readonly int DeathHash = Animator.StringToHash("Death");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _baseEnemy = GetComponent<BaseEnemy>();
            _animator = GetComponent<Animator>();
        }

        public void ApplyKnockbackAndKill(Vector2 force, float duration = 0.35f)
        {
            if (_isDying) return;
            _isDying = true;

            if (_knockbackRoutine != null)
                StopCoroutine(_knockbackRoutine);

            _knockbackRoutine = StartCoroutine(KnockbackAndKillRoutine(force, duration));
        }

        public void InstantKill()
        {
            if (_isDying) return;
            _isDying = true;
            DisableComponents();
            Destroy(gameObject);
        }

        private IEnumerator KnockbackAndKillRoutine(Vector2 force, float duration)
        {
            IsInKnockback = true;

            DisableComponents();

            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(duration);

            _rb.linearVelocity = Vector2.zero;
            _rb.simulated = false;

            if (_animator != null)
            {
                _animator.SetTrigger(HurtHash);
                _animator.SetTrigger(DeathHash);
            }

            yield return new WaitForSeconds(deathAnimationDuration);

            Destroy(gameObject);
        }

        private void DisableComponents()
        {
            if (_baseEnemy != null)
                _baseEnemy.enabled = false;

            var attackComp = GetComponent<EnemyAttack>();
            if (attackComp != null)
                attackComp.enabled = false;

            var col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
}