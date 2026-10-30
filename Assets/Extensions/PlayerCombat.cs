using UnityEngine;

namespace Extensions
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Kick Settings")] [SerializeField]
        private Transform attackPoint;

        [SerializeField] private float attackRadius = 0.8f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Vector2 knockbackForce = new Vector2(8f, 4f);

        public void PerformKickHit()
        {
            if (attackPoint == null) return;

            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            foreach (Collider2D hit in hits)
            {
                float kickDir = (hit.transform.position.x - transform.position.x) >= 0 ? 1f : -1f;
                Vector2 force = new Vector2(knockbackForce.x * kickDir, knockbackForce.y);

                EnemyKnockback enemyKb = hit.GetComponentInParent<EnemyKnockback>();
                if (enemyKb != null)
                {
                    // Launches enemy first, then triggers Hurt -> Death on settling
                    enemyKb.ApplyKnockbackAndKill(force, 0.3f);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}