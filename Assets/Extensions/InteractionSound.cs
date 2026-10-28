using UnityEngine;

namespace Extensions
{
    // Receives the same Animation Event as Framework.Animation.AnimationEventReceiver
    // (Unity calls matching method names on every component on the GameObject),
    // so this hooks the existing Action/Interaction clip without touching Framework.
    public class InteractionSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume = 0.7f;

        public void OnInteractionEvent(string actionName)
        {
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
    }
}
