using UnityEngine;

namespace Extensions
{
    // Sits on the same GameObject as Framework.Core.Level.LevelExitTrigger; Unity calls
    // OnTriggerEnter2D on every component that defines it, so this reacts to the same
    // trigger event without modifying the Framework script.
    public class LevelExitSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume = 0.7f;
        private bool _played;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_played) return;
            if (!other.CompareTag("Player")) return;
            _played = true;
            if (clip != null) AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
    }
}
