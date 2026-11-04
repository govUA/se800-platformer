using UnityEngine;
using Framework.Core.Motor;
using Framework.Core.Sensors;

namespace Framework.Animation
{
    //Виглядає як річ яка ускладнить розширення доволі сильно,
    //що б додати якусь поведінку треба буде лізти в наслідників
    /*
    AnimationBoolHash допускає тільки один bool. Атака, напрямок прицілювання, інтенсивність руху чи кілька одночасних ознак потребують інших параметрів. Контракт поведінки також змушує gameplay знати про параметри Animator.
    Анімація оновлюється через зміну AI-поведінки. Усередині тієї самої поведінки стан може змінитися, але bridge про це не дізнається. Уже є наслідок: ChaseBehaviour викликає SetBehaviour(this) заради оновлення анімації, повторно запускаючи Exit/Enter.
    У EnemyAnimationBridge основна логіка закрита в private методах. Нащадок не може доповнити Update або обробку зміни поведінки через override і base. Доведеться дублювати логіку або переробляти клас.
    ActorAnimationBridge задає фіксований набір Hurt/Death/Action. Для нових дій немає узгодженого способу розширення. У проєкті EnemyAttack уже працює з Animator напряму.
    Базовий клас вимагає motor і sensors для всіх нащадків, хоча, наприклад, стаціонарному ворогу може бути потрібна лише анімація атаки.
    */
    [RequireComponent(typeof(Animator))]
    public abstract class ActorAnimationBridge : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        protected Animator animator;

        [SerializeField] protected ActorMotor2D motor;
        [SerializeField] protected EnvironmentDetector2D sensors;

        protected virtual void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
            //Або [Serialized] або GetComponent - можемо загубити компоненту і буде енприємно
            if (motor == null) motor = GetComponentInParent<ActorMotor2D>();
            if (sensors == null) sensors = GetComponentInParent<EnvironmentDetector2D>();
        }

        public virtual void TriggerHurt()
        {
            //Буде тихо не працювати - найгірше що можемо отримати, помилка в цьому випадку не така страшна
            if (animator != null) animator.SetTrigger(AnimationHashes.Hurt);
        }

        public virtual void TriggerDeath()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.Death);
        }

        public  void TriggerAction()
        {
            if (animator != null) animator.SetTrigger(AnimationHashes.Action);
        }
    }
}