using UnityEngine;
using UnityEngine.InputSystem;

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
            if (targetBridge == null || Keyboard.current == null) return;

            if (Keyboard.current[hurtKey].wasPressedThisFrame) targetBridge.TriggerHurt();
            if (Keyboard.current[deathKey].wasPressedThisFrame) targetBridge.TriggerDeath();
            if (Keyboard.current[actionKey].wasPressedThisFrame) targetBridge.TriggerAction();
        }
    }
}