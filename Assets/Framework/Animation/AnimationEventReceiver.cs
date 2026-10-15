using UnityEngine;

namespace Framework.Animation
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        public void OnInteractionEvent(string actionName)
        {
            Debug.Log($"[AnimationEvent] Interaction dispatched: {actionName} at timestamp {Time.time:F2}s");
        }
    }
}