using Framework.Core.StateMachine;

namespace Framework.Player
{
    public abstract class BasePlayerState : IState
    {
        protected readonly PlayerContext Context;

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