using UnityEngine;

namespace Framework.Enemy
{
    public abstract class EnemyBehaviourProvider : MonoBehaviour
    {
        //Не подобається мені такий провайдер, тут заради параметрів ми сторюємо зайвий прошарок 
        //Можна прибрати повністю і тягнути собі Скріптабл з параметрами з проєкту, ще й того вигляду який потрібен стану - легко буде розширяти
        public abstract IEnemyBehaviour CreateBehaviour(BaseEnemy enemy); //Виглядає як параметр який непотрібен
    }
}