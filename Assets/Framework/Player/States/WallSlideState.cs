using UnityEngine;

namespace Framework.Player.States
{
    public class WallSlideState : BasePlayerState
    {
        private int _lastWallDir;

        public WallSlideState(PlayerContext context) : base(context)
        {
        }

        public override void Enter()
        {
            Context.Motor.SetGravityMultiplier(0f);
            _lastWallDir = Context.Sensors.WallDirection;
        }

        public override void Tick()
        {
            if (Context.Sensors.WallDirection != 0)
            {
                _lastWallDir = Context.Sensors.WallDirection;
            }

            // 1. Jump Kick
            if (Context.Input.JumpPressed || Context.Controller.HasBufferedJump)
            {
                Context.Controller.ConsumeJumpBuffer();

                int kickDir = _lastWallDir != 0 ? _lastWallDir : Context.Sensors.WallDirection;
                Vector2 wallJumpVector = new Vector2(
                    -kickDir * Context.Stats.wallJumpForce,
                    Context.Stats.InitialJumpVelocity
                );

                Context.Motor.ApplyImpulse(wallJumpVector);
                Context.Controller.AirborneState.TriggerWallJump(Context.Stats.wallJumpInputLockout);
                Context.Controller.ChangeState(Context.Controller.AirborneState);
                return;
            }

            // 2. Landed check
            if (Context.Sensors.IsGrounded)
            {
                Context.Controller.ChangeState(Context.Controller.GroundedState);
                return;
            }

            // 3. Detach ONLY when off the wall OR when pushing intentionally away
            bool isSteeringAway = Context.Input.HorizontalInput != 0 && Context.Input.HorizontalInput == -_lastWallDir;
            if (!Context.Sensors.IsOnWall || isSteeringAway)
            {
                Context.Controller.ChangeState(Context.Controller.AirborneState);
                return;
            }
        }

        public override void FixedTick()
        {
            // Actively slide down rather than only clamping
            Context.Motor.SetVelocityY(-Context.Stats.wallSlideSpeed);
        }

        public override void Exit()
        {
            Context.Motor.SetGravityMultiplier(1f);
        }
    }
}