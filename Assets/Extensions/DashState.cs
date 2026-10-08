using UnityEngine;
using Framework.Core.StateMachine;
using Framework.Player;

namespace Extensions
{
    public class DashState : BasePlayerState
    {
        private readonly float _dashSpeed;
        private readonly float _dashDuration;

        private float _direction = 1f;
        private float _timer;

        public DashState(PlayerContext context, float dashSpeed, float dashDuration) : base(context)
        {
            _dashSpeed = dashSpeed;
            _dashDuration = dashDuration;
        }

        public void SetDirection(float direction)
        {
            _direction = direction >= 0f ? 1f : -1f;
        }

        public override void Enter()
        {
            _timer = _dashDuration;
            Context.Motor.SetGravityMultiplier(0f);
            Context.Motor.ApplyImpulse(new Vector2(_direction * _dashSpeed, 0f));
        }

        public override void Tick()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                IState landingState = Context.Sensors.IsGrounded
                    ? (IState)Context.Controller.GroundedState
                    : Context.Controller.AirborneState;
                Context.Controller.ChangeState(landingState);
            }
        }

        public override void FixedTick()
        {
            Context.Motor.SetVelocityX(_direction * _dashSpeed);
        }

        public override void Exit()
        {
            Context.Motor.SetGravityMultiplier(1f);
        }
    }
}
