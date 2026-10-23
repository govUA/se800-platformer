using UnityEngine;
using UnityEngine.InputSystem;

namespace Extensions
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;

        private bool _paused;

        private void Update()
        {
            if (gameOverPanel != null && gameOverPanel.activeSelf) return;
            if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;

            if (_paused) Resume();
            else Pause();
        }

        private void Pause()
        {
            _paused = true;
            if (pausePanel != null) pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            _paused = false;
            if (pausePanel != null) pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
