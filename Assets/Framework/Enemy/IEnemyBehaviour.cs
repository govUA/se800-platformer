namespace Framework.Enemy
{
    public interface IEnemyBehaviour
    {
        void Enter(BaseEnemy enemy);
        void Tick(BaseEnemy enemy);
        void FixedTick(BaseEnemy enemy);
        void Exit(BaseEnemy enemy);
    }
}