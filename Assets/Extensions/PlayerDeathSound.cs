using UnityEngine;

namespace Extensions
{
    // Polls the public IsDead flag PlayerDeathReceiver already exposes.
    public class PlayerDeathSound : MonoBehaviour
    {
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private float volume = 0.7f;

        private PlayerDeathReceiver _deathReceiver;
        private bool _wasDead;

        private void Awake()
        {
            _deathReceiver = GetComponent<PlayerDeathReceiver>();
        }

        private void Update()
        {
            if (_deathReceiver == null) return;
            bool isDead = _deathReceiver.IsDead;
            if (isDead && !_wasDead && deathClip != null)
            {
                AudioSource.PlayClipAtPoint(deathClip, transform.position, volume);
            }
            _wasDead = isDead;
        }
    }
}
