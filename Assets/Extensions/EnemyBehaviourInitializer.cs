using UnityEngine;
using Framework.Enemy;
using Framework.Player;

namespace Extensions
{
    public enum CustomEnemyBehaviourType
    {
        Patrol,
        Chase,
        JumpToPlayer
    }

    [RequireComponent(typeof(BaseEnemy))]
    public class EnemyBehaviourInitializer : MonoBehaviour
    {
        [SerializeField] private CustomEnemyBehaviourType behaviourType = CustomEnemyBehaviourType.Chase;
        [SerializeField] private Transform player;

        [Header("Chase Settings")]
        [SerializeField] private float chaseDetectionRange = 6f;
        [SerializeField] private float chaseSpeedMultiplier = 1.6f;

        [Header("Jump To Player Settings")]
        [SerializeField] private float jumpTriggerRange = 4f;
        [SerializeField] private float jumpCooldown = 1.5f;
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float jumpHeight = 9f;

        private void Start()
        {
            if (player == null)
            {
                PlayerController playerController = FindFirstObjectByType<PlayerController>();
                if (playerController != null) player = playerController.transform;
            }

            BaseEnemy enemy = GetComponent<BaseEnemy>();

            switch (behaviourType)
            {
                case CustomEnemyBehaviourType.Chase:
                    enemy.SetBehaviour(new ChaseBehaviour(player, chaseDetectionRange, chaseSpeedMultiplier));
                    break;
                case CustomEnemyBehaviourType.JumpToPlayer:
                    enemy.SetBehaviour(new JumpToPlayerBehaviour(player, jumpTriggerRange, jumpCooldown, jumpForce,
                        jumpHeight));
                    break;
                case CustomEnemyBehaviourType.Patrol:
                default:
                    break;
            }
        }
    }
}
