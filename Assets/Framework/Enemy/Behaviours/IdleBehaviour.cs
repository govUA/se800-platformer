using UnityEngine;

namespace Framework.Enemy
{
    public class IdleBehaviour : IEnemyBehaviour
    {
        private float _timer;
        private readonly float _duration;

        public IdleBehaviour(float duration = 2f)
        {
            _duration = duration;
        }

        public void Enter(BaseEnemy enemy)
        {
            _timer = _duration;
            enemy.Motor.SetVelocityX(0f);
        }

        public void Tick(BaseEnemy enemy)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                enemy.FlipDirection();
                enemy.SetBehaviour(new PatrolBehaviour());
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
        }

        public void Exit(BaseEnemy enemy)
        {
        }
    }
}