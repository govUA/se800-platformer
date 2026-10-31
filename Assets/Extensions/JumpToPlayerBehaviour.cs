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

        private EnemyAttack _attack;
        private float _cooldownTimer;
        private float _airTargetVelocityX;
        private bool _isAirborne;

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
            _isAirborne = false;
            _airTargetVelocityX = 0f;
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

            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

            // Reset airborne tracking when enemy lands
            if (enemy.Sensors.IsGrounded)
            {
                _isAirborne = false;
            }

            if (_player == null || !enemy.Sensors.IsGrounded) return;

            float distance = _player.position.x - enemy.transform.position.x;
            float absDistance = Mathf.Abs(distance);
            bool playerInRange = absDistance <= _triggerRange;
            int dirToPlayer = distance >= 0f ? 1 : -1;

            if (playerInRange)
            {
                // 1. Keep enemy oriented toward player
                if (dirToPlayer != enemy.FacingDirection)
                {
                    enemy.ForceFlipDirection();
                }

                // 2. Launch targeted pursuit jump
                if (_cooldownTimer <= 0f)
                {
                    _cooldownTimer = _jumpCooldown;
                    _isAirborne = true;

                    // Approximate flight time based on vertical impulse and motor gravity
                    // t ≈ (2 * Vy) / g
                    float gravity = 30f;
                    float estimatedHangTime = (2f * _jumpHeight) / Mathf.Max(gravity, 10f);

                    // Required horizontal speed to close the distance
                    float requiredSpeed = absDistance / Mathf.Max(estimatedHangTime, 0.3f);

                    // Clamp between regular move speed and maximum jump force
                    float horizontalSpeed = Mathf.Clamp(requiredSpeed, enemy.MoveSpeed * 1.5f, _jumpForce);

                    _airTargetVelocityX = enemy.FacingDirection * horizontalSpeed;

                    enemy.Motor.ApplyImpulse(new Vector2(_airTargetVelocityX, _jumpHeight));
                    return;
                }
            }
            else
            {
                // 3. Normal patrol turn at wall/ledge when player is out of range
                if (enemy.Sensors.IsOnWall || enemy.IsLedgeAhead())
                {
                    enemy.FlipDirection();
                }
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
            if (_attack != null && _attack.IsAttacking) return;

            // If airborne from a jump, continuously enforce horizontal momentum so it reaches the player
            if (!enemy.Sensors.IsGrounded)
            {
                if (_isAirborne)
                {
                    enemy.Motor.SetVelocityX(_airTargetVelocityX);
                }

                return;
            }

            // Grounded: Stop if waiting at a ledge, otherwise move towards facing direction
            if (enemy.IsLedgeAhead())
            {
                enemy.Motor.SetVelocityX(0f);
                return;
            }

            enemy.Motor.SetVelocityX(enemy.FacingDirection * enemy.MoveSpeed);
        }

        public void Exit(BaseEnemy enemy)
        {
            _isAirborne = false;
            enemy.Motor.SetVelocityX(0f);
        }
    }
}