using UnityEngine;
using UnityEngine.SceneManagement;
using Framework.Player;

namespace Framework.Core.Level
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelExitTrigger : MonoBehaviour
    {
        [SerializeField] private string nextSceneName;
        [SerializeField] private bool resetToFirstIfMissing = false;

        private bool _isTriggered;

        private void Awake()
        {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTriggered) return;

            // Confirm trigger source is the player
            //Дві перевірки одного і того ж самого + можна матрицею взаємодії без перевірок вирішити
            if (other.GetComponentInParent<PlayerController>() != null || other.CompareTag("Player"))
            {
                _isTriggered = true;
                LoadNextLevel();
            }
        }

        private void LoadNextLevel()
        {
            //Не думаю що це має бути відповідальністю LevelExitTrigger, бо в нас може бути багато джерел зміни сцени
            if (!string.IsNullOrEmpty(nextSceneName) && Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else if (resetToFirstIfMissing)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log($"[LevelExitTrigger] Target destination reached. Scene '{nextSceneName}' not loaded.");
            }
        }
    }
}