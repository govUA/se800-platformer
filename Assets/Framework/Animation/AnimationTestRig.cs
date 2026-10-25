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
            if (Keyboard.current == null) return;

            var playerDeath = targetBridge != null
                ? targetBridge.GetComponentInParent<PlayerDeathReceiver>()
                : null;

            // 'K' Key: Instant Death without Hurt recoil
            if (Keyboard.current[deathKey].wasPressedThisFrame)
            {
                if (playerDeath != null)
                {
                    playerDeath.KillAndKnockback(Vector2.zero, skipHurt: true);
                    return;
                }

                targetBridge?.TriggerDeath();
            }

            // 'H' Key: Full damage hit sequence (Hurt -> Death)
            if (Keyboard.current[hurtKey].wasPressedThisFrame)
            {
                if (playerDeath != null)
                {
                    playerDeath.KillAndKnockback(new Vector2(-4f, 7f), skipHurt: false);
                    return;
                }

                targetBridge?.TriggerHurt();
            }

            // 'J' Key: Action / Kick
            if (targetBridge != null && Keyboard.current[actionKey].wasPressedThisFrame)
            {
                targetBridge.TriggerAction();
            }
        }
    }
}