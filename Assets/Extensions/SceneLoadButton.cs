using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Extensions
{
    [RequireComponent(typeof(Button))]
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        private Button _button;
        
        //Дуже небезепечна поведінка, яка може привести до небажаного продовження життя цього обʼєкту
        //Правило таке, якщо ми підписались на щось, маємо відписатись
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(sceneName);
            });
        }

        //Ось так
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
