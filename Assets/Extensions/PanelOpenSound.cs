using UnityEngine;

namespace Extensions
{
    public class PanelOpenSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume = 0.7f;

        private void OnEnable()
        {
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume);
        }
    }
}
