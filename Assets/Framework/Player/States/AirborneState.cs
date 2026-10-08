using UnityEngine;

namespace Framework.Player.States
{
    public class AirborneState : BasePlayerState
    {
        private float _coyoteTimer;
        private float _wallJumpLockoutTimer;

        public AirborneState(PlayerContext context) : base(context)
        {
        }

        public void TriggerWallJump(float lockoutDuration)
        {
            _wallJumpLockoutTimer = lockoutDuration;
        }

        public override void Enter()
        {
            // Allow coyote jump only if transitioning while still having upward/neutral initial entry from ground
            _coyoteTimer = Context.Sensors.IsGrounded ? Context.Stats.coyoteTime : 0f;
        }

        public override void Tick()
        {
            _coyoteTimer -= Time.deltaTime;
            if (_wallJumpLockoutTimer > 0f) _wallJumpLockoutTimer -= Time.deltaTime;

            // Landing check
            if (Context.Sensors.IsGrounded && Context.Motor.Velocity.y <= 0.05f)
            {
                Context.Controller.ChangeState(Context.Controller.GroundedState);
                return;
            }

            // Direct Jump Kick off wall while in air
            if (Context.Input.JumpPressed && Context.Sensors.IsOnWall)
            {
                Vector2 wallJumpVector = new Vector2(
                    -Context.Sensors.WallDirection * Context.Stats.wallJumpForce,
                    Context.Stats.InitialJumpVelocity
                );
                Context.Motor.ApplyImpulse(wallJumpVector);
                TriggerWallJump(Context.Stats.wallJumpInputLockout);
                return;
            }

            // Wall slide transition:
            // Only when descending/neutral, and player is NOT actively pressing away from the wall
            bool isSteeringAway = Context.Input.HorizontalInput != 0 &&
                                  Context.Input.HorizontalInput != Context.Sensors.WallDirection;
            if (_wallJumpLockoutTimer <= 0f &&
                Context.Sensors.IsOnWall &&
                Context.Motor.Velocity.y <= 0.1f &&
                !isSteeringAway)
            {
                Context.Controller.ChangeState(Context.Controller.WallSlideState);
                return;
            }

            // Coyote Jump
            if (Context.Input.JumpPressed && _coyoteTimer > 0f)
            {
                _coyoteTimer = 0f;
                Context.Motor.SetVelocityY(Context.Stats.InitialJumpVelocity);
                return;
            }

            // Buffer Jump Input if pressed early near ground
            if (Context.Input.JumpPressed)
            {
                Context.Controller.BufferJump();
            }

            // Variable Jump Height
            if (Context.Input.JumpReleased && Context.Motor.Velocity.y > 0f)
            {
                Context.Motor.SetGravityMultiplier(Context.Stats.jumpCutGravityMultiplier);
            }
            else if (Context.Motor.Velocity.y < 0f)
            {
                Context.Motor.SetGravityMultiplier(Context.Stats.fallGravityMultiplier);
            }
        }

        public override void FixedTick()
        {
            // Suppress target speed override during initial kick impulse
            if (_wallJumpLockoutTimer > 0f) return;

            float targetSpeed = Context.Input.HorizontalInput * Context.Stats.walkSpeed;
            float airRate = (Mathf.Abs(targetSpeed) > 0.01f ? Context.Stats.acceleration : Context.Stats.deceleration) *
                            Context.Stats.airControlFactor;
            Context.Motor.MoveHorizontal(targetSpeed, airRate);
        }
    }
}