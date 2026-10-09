using UnityEngine;
using Framework.Player;

namespace Extensions
{
    [RequireComponent(typeof(PlayerController))]
    public class DoubleJumpAbility : MonoBehaviour
    {
        [SerializeField] private int extraJumps = 1;
        [SerializeField] private float extraJumpVelocityMultiplier = 0.9f;

        private PlayerController _controller;
        private int _remainingJumps;
        private float _airborneTimer;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _remainingJumps = extraJumps;
        }

        private void Start()
        {
            _controller.Context.Sensors.OnGrounded += HandleGrounded;
            _controller.OnStateChanged += HandleStateChanged;
        }

        private void OnDestroy()
        {
            _controller.Context.Sensors.OnGrounded -= HandleGrounded;
            _controller.OnStateChanged -= HandleStateChanged;
        }

        private void HandleGrounded()
        {
            _remainingJumps = extraJumps;
        }

        private void HandleStateChanged(Framework.Core.StateMachine.IState newState)
        {
            if (newState == _controller.AirborneState) _airborneTimer = 0f;
        }

        private void Update()
        {
            bool isAirborne = _controller.CurrentState == _controller.AirborneState;
            if (!isAirborne) return;

            _airborneTimer += Time.deltaTime;

            bool coyoteWindowPassed = _airborneTimer > _controller.Context.Stats.coyoteTime;
            bool touchingWall = _controller.Context.Sensors.IsOnWall;

            if (coyoteWindowPassed && !touchingWall && _remainingJumps > 0 && _controller.Context.Input.JumpPressed)
            {
                _remainingJumps--;
                float velocity = _controller.Context.Stats.InitialJumpVelocity * extraJumpVelocityMultiplier;
                _controller.Context.Motor.SetVelocityY(velocity);
            }
        }
    }
}
