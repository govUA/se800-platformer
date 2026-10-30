using UnityEngine;

namespace Extensions
{
    // Polls the public state already exposed by EnemyAttack/EnemyKnockback instead of
    // modifying those scripts, so it adds audio without touching teammate code.
    public class EnemyCombatSound : MonoBehaviour
    {
        [SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioClip knockbackClip;
        [SerializeField] private float volume = 0.7f;

        private EnemyAttack _attack;
        private EnemyKnockback _knockback;
        private bool _wasAttacking;
        private bool _wasDying;

        private void Awake()
        {
            _attack = GetComponent<EnemyAttack>();
            _knockback = GetComponent<EnemyKnockback>();
        }

        private void Update()
        {
            if (_attack != null)
            {
                bool isAttacking = _attack.IsAttacking;
                if (isAttacking && !_wasAttacking && attackClip != null)
                {
                    AudioSource.PlayClipAtPoint(attackClip, transform.position, volume);
                }
                _wasAttacking = isAttacking;
            }

            if (_knockback != null)
            {
                bool isDying = _knockback.IsDying;
                if (isDying && !_wasDying && knockbackClip != null)
                {
                    AudioSource.PlayClipAtPoint(knockbackClip, transform.position, volume);
                }
                _wasDying = isDying;
            }
        }
    }
}
