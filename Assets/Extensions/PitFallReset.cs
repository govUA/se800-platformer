using Framework.Player;
using UnityEngine;

namespace Extensions
{
    [RequireComponent(typeof(Collider2D))]
    public class PitFallReset : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;

        private bool _triggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggered) return;
            if (other.GetComponentInParent<PlayerController>() == null && !other.CompareTag("Player")) return;

            _triggered = true;
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
