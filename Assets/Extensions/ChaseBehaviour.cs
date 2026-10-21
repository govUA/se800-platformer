using UnityEngine;
using Framework.Enemy;

namespace Extensions
{
    public class ChaseBehaviour : IEnemyBehaviour
    {
        private static readonly int ChasingHash = Animator.StringToHash("IsChasing");

        private readonly Transform _player;
        private readonly float _detectionRange;
        private readonly float _chaseSpeedMultiplier;

        private bool _isChasing;

        public int AnimationBoolHash => _isChasing ? ChasingHash : 0;

        public ChaseBehaviour(Transform player, float detectionRange, float chaseSpeedMultiplier = 1.6f)
        {
            _player = player;
            _detectionRange = detectionRange;
            _chaseSpeedMultiplier = chaseSpeedMultiplier;
        }

        public void Enter(BaseEnemy enemy)
        {
        }

        public void Tick(BaseEnemy enemy)
        {
            float distance = _player != null ? _player.position.x - enemy.transform.position.x : 0f;
            bool wasChasing = _isChasing;
            _isChasing = _player != null && Mathf.Abs(distance) <= _detectionRange;

            // If chase state toggled, re-notify the enemy to update the bridge
            if (wasChasing != _isChasing)
            {
                enemy.SetBehaviour(this);
            }

            int desiredDirection = _isChasing ? (int)Mathf.Sign(distance) : enemy.FacingDirection;

            bool blocked = enemy.Sensors.IsOnWall || enemy.IsLedgeAhead();
            if (blocked && desiredDirection == enemy.FacingDirection)
            {
                enemy.FlipDirection();
                return;
            }

            if (desiredDirection != 0 && desiredDirection != enemy.FacingDirection)
            {
                enemy.FlipDirection();
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
            float speed = enemy.MoveSpeed * (_isChasing ? _chaseSpeedMultiplier : 1f);
            enemy.Motor.SetVelocityX(enemy.FacingDirection * speed);
        }

        public void Exit(BaseEnemy enemy)
        {
            enemy.Motor.SetVelocityX(0f);
        }
    }
}