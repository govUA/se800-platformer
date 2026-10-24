using System.Collections;
using Framework.Enemy;
using UnityEngine;

namespace Extensions
{
    public class EnemyKnockback : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private BaseEnemy _baseEnemy;
        private Coroutine _knockbackRoutine;

        public bool IsInKnockback { get; private set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _baseEnemy = GetComponent<BaseEnemy>();
        }

        public void ApplyKnockback(Vector2 force, float duration = 0.25f)
        {
            if (_knockbackRoutine != null)
                StopCoroutine(_knockbackRoutine);

            _knockbackRoutine = StartCoroutine(KnockbackRoutine(force, duration));
        }

        private IEnumerator KnockbackRoutine(Vector2 force, float duration)
        {
            IsInKnockback = true;

            // 1. Temporarily pause the enemy's autonomous walking
            if (_baseEnemy != null)
                _baseEnemy.enabled = false;

            // 2. Clear current momentum and apply the kick impulse
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(force, ForceMode2D.Impulse);

            // 3. Wait for physics to carry the enemy
            yield return new WaitForSeconds(duration);

            // 4. Restore enemy movement
            if (_baseEnemy != null)
                _baseEnemy.enabled = true;

            IsInKnockback = false;
            _knockbackRoutine = null;
        }
    }
}