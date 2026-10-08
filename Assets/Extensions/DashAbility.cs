using UnityEngine;
using UnityEngine.InputSystem;
using Framework.Player;

namespace Extensions
{
    [RequireComponent(typeof(PlayerController))]
    public class DashAbility : MonoBehaviour
    {
        [SerializeField] private float dashSpeed = 18f;
        [SerializeField] private float dashDuration = 0.18f;
        [SerializeField] private float dashCooldown = 0.6f;

        private PlayerController _controller;
        private DashState _dashState;
        private float _lastFacing = 1f;
        private float _cooldownTimer;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void Start()
        {
            _dashState = new DashState(_controller.Context, dashSpeed, dashDuration);
        }

        private void Update()
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

            float horizontal = _controller.Context.Input.HorizontalInput;
            if (Mathf.Abs(horizontal) > 0.01f) _lastFacing = Mathf.Sign(horizontal);

            bool dashPressed = Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame;
            bool canDash = _cooldownTimer <= 0f &&
                           _controller.CurrentState != _dashState &&
                           _controller.CurrentState != _controller.WallSlideState;

            if (dashPressed && canDash)
            {
                _dashState.SetDirection(_lastFacing);
                _cooldownTimer = dashCooldown;
                _controller.ChangeState(_dashState);
            }
        }
    }
}
