using UnityEngine;
using Extensions;

namespace Framework.Enemy
{
    public class PatrolBehaviour : IEnemyBehaviour
    {
        private EnemyAttack _attack;

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

            // Reverse direction on wall collision or ledge detection
            if (enemy.Sensors.IsOnWall || enemy.IsLedgeAhead())
            {
                enemy.FlipDirection();
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
            if (_attack != null && _attack.IsAttacking) return;

            enemy.Motor.SetVelocityX(enemy.FacingDirection * enemy.MoveSpeed);
        }

        public void Exit(BaseEnemy enemy)
        {
            enemy.Motor.SetVelocityX(0f);
        }
    }
}