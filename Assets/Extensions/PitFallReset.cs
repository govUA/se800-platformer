using UnityEngine;

namespace Extensions
{
    [RequireComponent(typeof(Collider2D))]
    public class PitFallReset : MonoBehaviour
    {
        [SerializeField] private Vector2 fallImpulse = new Vector2(0f, 10f);

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var deathReceiver = other.GetComponentInParent<PlayerDeathReceiver>();
            if (deathReceiver != null && !deathReceiver.IsDead)
            {
                deathReceiver.KillAndKnockback(fallImpulse);
            }
        }
    }
}