using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Extensions
{
    [RequireComponent(typeof(Button))]
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(sceneName);
            });
        }
    }
}
