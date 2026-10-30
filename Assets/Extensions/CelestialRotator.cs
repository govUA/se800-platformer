using UnityEngine;

namespace Extensions
{
    public class CelestialRotator : MonoBehaviour
    {
        [Tooltip("Degrees per second. Positive = counter-clockwise, Negative = clockwise.")] [SerializeField]
        private float rotationSpeed = 2f;

        private void Update()
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }
}