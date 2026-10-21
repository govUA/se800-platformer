using Framework.Core.StateMachine;

namespace Framework.Player
{
    public abstract class BasePlayerState : IState
    {
        protected readonly PlayerContext Context;

        // Returns an Animator parameter hash if this state maps to an active boolean flag; returns 0 if none.
        public virtual int AnimationBoolHash => 0;

        protected BasePlayerState(PlayerContext context)
        {
            Context = context;
        }

        public virtual void Enter()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void FixedTick()
        {
        }

        public virtual void Exit()
        {
        }
    }
}