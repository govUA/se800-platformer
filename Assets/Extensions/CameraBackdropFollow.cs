using UnityEngine;

namespace Extensions
{
    public class CameraBackdropFollow : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float zDepth = 5f;

        private void Start()
        {
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (cameraTransform == null) return;
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, zDepth);
        }
    }
}