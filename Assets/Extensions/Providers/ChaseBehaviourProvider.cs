using UnityEngine;
using Framework.Enemy;
using Framework.Player;

namespace Extensions.Providers
{
    public class ChaseBehaviourProvider : EnemyBehaviourProvider
    {
        [SerializeField] private Transform player;
        [SerializeField] private float chaseDetectionRange = 6f;
        [SerializeField] private float chaseSpeedMultiplier = 1.6f;

        public override IEnemyBehaviour CreateBehaviour(BaseEnemy enemy)
        {
            if (player == null)
            {
                PlayerController playerController = Object.FindFirstObjectByType<PlayerController>();
                if (playerController != null) player = playerController.transform;
            }

            return new ChaseBehaviour(player, chaseDetectionRange, chaseSpeedMultiplier);
        }
    }
}