using UnityEngine;
using Framework.Enemy;

namespace Extensions
{
    public class JumpToPlayerBehaviour : IEnemyBehaviour
    {
        private readonly Transform _player;
        private readonly float _triggerRange;
        private readonly float _jumpCooldown;
        private readonly float _jumpForce;
        private readonly float _jumpHeight;

        private float _cooldownTimer;

        public JumpToPlayerBehaviour(Transform player, float triggerRange, float jumpCooldown, float jumpForce,
            float jumpHeight)
        {
            _player = player;
            _triggerRange = triggerRange;
            _jumpCooldown = jumpCooldown;
            _jumpForce = jumpForce;
            _jumpHeight = jumpHeight;
        }

        public void Enter(BaseEnemy enemy)
        {
            _cooldownTimer = 0f;
        }

        public void Tick(BaseEnemy enemy)
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

            if (enemy.Sensors.IsOnWall || enemy.IsLedgeAhead())
            {
                enemy.FlipDirection();
            }

            if (_player == null || !enemy.Sensors.IsGrounded) return;

            float distance = _player.position.x - enemy.transform.position.x;
            bool playerInRange = Mathf.Abs(distance) <= _triggerRange;

            if (playerInRange && _cooldownTimer <= 0f)
            {
                int jumpDirection = distance >= 0f ? 1 : -1;
                if (jumpDirection != enemy.FacingDirection) enemy.FlipDirection();

                _cooldownTimer = _jumpCooldown;
                enemy.Motor.ApplyImpulse(new Vector2(jumpDirection * _jumpForce, _jumpHeight));
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
            if (!enemy.Sensors.IsGrounded) return;
            enemy.Motor.SetVelocityX(enemy.FacingDirection * enemy.MoveSpeed);
        }

        public void Exit(BaseEnemy enemy)
        {
            enemy.Motor.SetVelocityX(0f);
        }
    }
}
