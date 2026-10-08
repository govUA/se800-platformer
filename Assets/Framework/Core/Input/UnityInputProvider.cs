using UnityEngine;
using UnityEngine.InputSystem; // Обов'язково додаємо цей простір імен

namespace Framework.Core.Input
{
    public class UnityInputProvider : MonoBehaviour, IInputProvider
    {
        public float HorizontalInput
        {
            get
            {
                var keyboard = Keyboard.current;
                if (keyboard == null) return 0f;

                float left = keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? -1f : 0f;
                float right = keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f;

                return left + right;
            }
        }

        public bool JumpPressed => Keyboard.current?.spaceKey.wasPressedThisFrame ?? false;
        public bool JumpHeld => Keyboard.current?.spaceKey.isPressed ?? false;
        public bool JumpReleased => Keyboard.current?.spaceKey.wasReleasedThisFrame ?? false;
    }
}