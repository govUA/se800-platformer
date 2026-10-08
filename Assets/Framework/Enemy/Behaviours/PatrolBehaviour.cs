using UnityEngine;

namespace Framework.Enemy
{
    public class PatrolBehaviour : IEnemyBehaviour
    {
        public void Enter(BaseEnemy enemy)
        {
        }

        public void Tick(BaseEnemy enemy)
        {
            // Reverse direction on wall collision or ledge detection
            if (enemy.Sensors.IsOnWall || enemy.IsLedgeAhead())
            {
                enemy.FlipDirection();
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
            enemy.Motor.SetVelocityX(enemy.FacingDirection * enemy.MoveSpeed);
        }

        public void Exit(BaseEnemy enemy)
        {
            enemy.Motor.SetVelocityX(0f);
        }
    }
}