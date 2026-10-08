using UnityEngine;

namespace Framework.Player.States
{
    public class GroundedState : BasePlayerState
    {
        public GroundedState(PlayerContext context) : base(context)
        {
        }

        public override void Enter()
        {
            Context.Motor.SetGravityMultiplier(1f);
        }

        public override void Tick()
        {
            // Ground to Air transition (walking off ledge)
            if (!Context.Sensors.IsGrounded)
            {
                Context.Controller.ChangeState(Context.Controller.AirborneState);
                return;
            }

            // Jump handling
            if (Context.Input.JumpPressed || Context.Controller.HasBufferedJump)
            {
                Context.Controller.ConsumeJumpBuffer();
                ExecuteJump();
                return;
            }
        }

        public override void FixedTick()
        {
            float targetSpeed = Context.Input.HorizontalInput * Context.Stats.walkSpeed;
            float rate = Mathf.Abs(targetSpeed) > 0.01f ? Context.Stats.acceleration : Context.Stats.deceleration;
            Context.Motor.MoveHorizontal(targetSpeed, rate);
        }

        private void ExecuteJump()
        {
            Context.Motor.SetVelocityY(Context.Stats.InitialJumpVelocity);
            Context.Controller.ChangeState(Context.Controller.AirborneState);
        }
    }
}