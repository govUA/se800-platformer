using UnityEngine;

namespace Framework.Enemy
{
    public abstract class EnemyBehaviourProvider : MonoBehaviour
    {
        public abstract IEnemyBehaviour CreateBehaviour(BaseEnemy enemy);
    }
}