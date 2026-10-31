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

        private EnemyAttack _attack;
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
            _attack = enemy.GetComponent<EnemyAttack>();
        }

        public void Tick(BaseEnemy enemy)
        {
            if (_attack != null)
            {
                if (_attack.IsAttacking) return;

                if (_attack.CanAttack())
                {
                    _attack.ExecuteAttack();
                    return;
                }
            }

            float distance = _player != null ? _player.position.x - enemy.transform.position.x : 0f;
            bool wasChasing = _isChasing;
            _isChasing = _player != null && Mathf.Abs(distance) <= _detectionRange;

            if (wasChasing != _isChasing)
            {
                enemy.SetBehaviour(this);
            }

            int desiredDirection = _isChasing ? (int)Mathf.Sign(distance) : enemy.FacingDirection;

            bool blocked = enemy.Sensors.IsOnWall || enemy.IsLedgeAhead();
            if (blocked && desiredDirection == enemy.FacingDirection)
            {
                if (enemy.CanFlip) enemy.FlipDirection();
                return;
            }

            if (desiredDirection != 0 && desiredDirection != enemy.FacingDirection)
            {
                if (enemy.CanFlip) enemy.FlipDirection();
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
            if (_attack != null && _attack.IsAttacking) return;

            float speed = enemy.MoveSpeed * (_isChasing ? _chaseSpeedMultiplier : 1f);
            enemy.Motor.SetVelocityX(enemy.FacingDirection * speed);
        }

        public void Exit(BaseEnemy enemy)
        {
            enemy.Motor.SetVelocityX(0f);
        }
    }
}