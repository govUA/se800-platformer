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
            Vector2 groundVel = Context.Sensors.GroundVelocity;
            Vector2 normal = Context.Sensors.GroundNormal;
            bool isOnSlope = Mathf.Abs(normal.x) > 0.05f && normal.y > 0.5f;

            // On moving platforms with vertical speed
            if (groundVel.y != 0f)
            {
                Context.Motor.SetVelocityY(groundVel.y);
            }
            else if (isOnSlope)
            {
                // Don't accumulate downward gravity while grounded on a slope
                if (Context.Motor.Velocity.y < 0f && Context.Input.HorizontalInput == 0f)
                {
                    // Rest on slope when no input is pressed
                    Context.Motor.SetVelocityY(0f);
                }
            }
            else if (Context.Motor.Velocity.y < 0f)
            {
                Context.Motor.SetVelocityY(0f);
            }

            float targetSpeed = Context.Input.HorizontalInput * Context.Stats.walkSpeed;
            float rate = Mathf.Abs(targetSpeed) > 0.01f ? Context.Stats.acceleration : Context.Stats.deceleration;

            if (isOnSlope)
            {
                Context.Motor.MoveOnSlope(targetSpeed, rate, normal, groundVel.x);
            }
            else
            {
                Context.Motor.MoveHorizontal(targetSpeed, rate, groundVel.x);
            }
        }

        private void ExecuteJump()
        {
            // Jump off platform retaining surface velocity
            float jumpY = Context.Stats.InitialJumpVelocity + Mathf.Max(0f, Context.Sensors.GroundVelocity.y);
            Context.Motor.SetVelocityY(jumpY);
            Context.Controller.ChangeState(Context.Controller.AirborneState);
        }
    }
}