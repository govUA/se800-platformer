using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    [RequireComponent(typeof(Button))]
    public class ButtonClickSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume = 0.7f;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (clip == null) return;
                AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume);
            });
        }
    }
}
