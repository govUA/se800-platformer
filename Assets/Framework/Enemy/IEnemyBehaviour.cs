namespace Framework.Enemy
{
    public interface IEnemyBehaviour
    {
        int AnimationBoolHash => 0;
        void Enter(BaseEnemy enemy);
        void Tick(BaseEnemy enemy);
        void FixedTick(BaseEnemy enemy);
        void Exit(BaseEnemy enemy);
    }
}