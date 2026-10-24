using UnityEngine;

namespace Extensions
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Kick Settings")] [SerializeField]
        private Transform attackPoint;

        [SerializeField] private float attackRadius = 0.8f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Vector2 knockbackForce = new Vector2(5f, 3f);

        // Called by an Animation Event during Player_Action
        public void PerformKickHit()
        {
            if (attackPoint == null) return;

            // Find all enemy colliders in reach
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            foreach (Collider2D hit in hits)
            {
                // 1. Trigger Enemy Hurt animation
                Animator enemyAnim = hit.GetComponent<Animator>();
                if (enemyAnim != null)
                {
                    enemyAnim.SetTrigger("Hurt");
                }

                // 2. Calculate direction from Player towards Enemy (more reliable than localScale)
                float kickDir = (hit.transform.position.x - transform.position.x) >= 0 ? 1f : -1f;
                Vector2 force = new Vector2(knockbackForce.x * kickDir, knockbackForce.y);

                // 3. Apply knockback through enemy stun controller if present
                EnemyKnockback enemyKb = hit.GetComponent<EnemyKnockback>();
                if (enemyKb != null)
                {
                    enemyKb.ApplyKnockback(force, 0.25f);
                }
                else
                {
                    // Fallback if component missing
                    Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        enemyRb.linearVelocity = Vector2.zero;
                        enemyRb.AddForce(force, ForceMode2D.Impulse);
                    }
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