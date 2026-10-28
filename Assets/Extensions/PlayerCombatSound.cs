using UnityEngine;

namespace Extensions
{
    // "PerformKickHit" matches the method name Extensions.PlayerCombat exposes for its
    // Animation Event; Unity calls every component with a matching method name on the
    // same GameObject, so this plays a sound without editing PlayerCombat itself.
    public class PlayerCombatSound : MonoBehaviour
    {
        [SerializeField] private AudioClip kickClip;
        [SerializeField] private float volume = 0.7f;

        public void PerformKickHit()
        {
            if (kickClip == null) return;
            AudioSource.PlayClipAtPoint(kickClip, transform.position, volume);
        }
    }
}
