using UnityEngine;

namespace Extensions
{
    [RequireComponent(typeof(Collider2D))]
    public class PitFallReset : MonoBehaviour
    {
        [SerializeField] private Vector2 fallImpulse = new Vector2(0f, 10f);

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerDeath = other.GetComponentInParent<PlayerDeathReceiver>();
            if (playerDeath != null && !playerDeath.IsDead)
            {
                playerDeath.KillAndKnockback(fallImpulse);
                return;
            }

            var enemyKnockback = other.GetComponentInParent<EnemyKnockback>();
            if (enemyKnockback != null)
            {
                enemyKnockback.InstantKill();
                return;
            }

            if (other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}