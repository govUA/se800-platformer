using UnityEngine;
using UnityEngine.InputSystem;
using Extensions;

namespace Framework.Animation
{
    public class AnimationTestRig : MonoBehaviour
    {
        [SerializeField] private ActorAnimationBridge targetBridge;
        [SerializeField] private Key hurtKey = Key.H;
        [SerializeField] private Key deathKey = Key.K;
        [SerializeField] private Key actionKey = Key.J;

        private void Update()
        {
            if (Keyboard.current == null || targetBridge == null) return;

            // 'K' Key: Instant Death without Hurt recoil
            if (Keyboard.current[deathKey].wasPressedThisFrame)
            {
                // If target is Player
                var playerDeath = targetBridge.GetComponentInParent<PlayerDeathReceiver>();
                if (playerDeath != null)
                {
                    playerDeath.KillAndKnockback(Vector2.zero, skipHurt: true);
                    return;
                }

                // If target is Enemy
                var enemyKb = targetBridge.GetComponentInParent<EnemyKnockback>();
                if (enemyKb != null)
                {
                    enemyKb.InstantKill();
                    return;
                }

                targetBridge.TriggerDeath();
            }

            // 'H' Key: Full damage hit sequence (Hurt -> Death)
            if (Keyboard.current[hurtKey].wasPressedThisFrame)
            {
                var playerDeath = targetBridge.GetComponentInParent<PlayerDeathReceiver>();
                if (playerDeath != null)
                {
                    playerDeath.KillAndKnockback(new Vector2(-4f, 7f), skipHurt: false);
                    return;
                }

                var enemyKb = targetBridge.GetComponentInParent<EnemyKnockback>();
                if (enemyKb != null)
                {
                    enemyKb.ApplyKnockbackAndKill(new Vector2(-6f, 3f), 0.3f);
                    return;
                }

                targetBridge.TriggerHurt();
            }

            // 'J' Key: Action / Kick
            if (Keyboard.current[actionKey].wasPressedThisFrame)
            {
                targetBridge.TriggerAction();
            }
        }
    }
}