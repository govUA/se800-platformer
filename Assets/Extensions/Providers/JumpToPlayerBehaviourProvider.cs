using UnityEngine;
using Framework.Enemy;
using Framework.Player;

namespace Extensions.Providers
{
    public class JumpToPlayerBehaviourProvider : EnemyBehaviourProvider
    {
        [SerializeField] private Transform player;
        [SerializeField] private float jumpTriggerRange = 4f;
        [SerializeField] private float jumpCooldown = 1.5f;
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float jumpHeight = 9f;

        public override IEnemyBehaviour CreateBehaviour(BaseEnemy enemy)
        {
            if (player == null)
            {
                PlayerController playerController = Object.FindFirstObjectByType<PlayerController>();
                if (playerController != null) player = playerController.transform;
            }

            return new JumpToPlayerBehaviour(player, jumpTriggerRange, jumpCooldown, jumpForce, jumpHeight);
        }
    }
}