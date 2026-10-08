using System;
using UnityEngine;
using Framework.Core.Input;
using Framework.Core.Motor;
using Framework.Core.Sensors;
using Framework.Core.StateMachine;
using Framework.Data;
using Framework.Player.States;

namespace Framework.Player
{
    [RequireComponent(typeof(ActorMotor2D), typeof(EnvironmentDetector2D), typeof(IInputProvider))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MovementStatsSO stats;

        public IState CurrentState { get; private set; }

        public GroundedState GroundedState { get; private set; }
        public AirborneState AirborneState { get; private set; }
        public WallSlideState WallSlideState { get; private set; }

        public PlayerContext Context { get; private set; }

        // Extension event to listen to state shifts
        public event Action<IState> OnStateChanged;

        private float _jumpBufferTimer;
        public bool HasBufferedJump => _jumpBufferTimer > 0f;

        private void Awake()
        {
            ActorMotor2D motor = GetComponent<ActorMotor2D>();
            EnvironmentDetector2D sensors = GetComponent<EnvironmentDetector2D>();
            IInputProvider input = GetComponent<IInputProvider>();

            motor.Initialize(stats);
            Context = new PlayerContext(motor, sensors, stats, input, this);

            GroundedState = new GroundedState(Context);
            AirborneState = new AirborneState(Context);
            WallSlideState = new WallSlideState(Context);
        }

        private void Start()
        {
            ChangeState(GroundedState);
        }

        private void Update()
        {
            if (_jumpBufferTimer > 0f) _jumpBufferTimer -= Time.deltaTime;
            CurrentState?.Tick();
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedTick();
        }

        public void ChangeState(IState newState)
        {
            if (newState == null || newState == CurrentState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
            OnStateChanged?.Invoke(CurrentState);
        }

        public void BufferJump()
        {
            _jumpBufferTimer = stats.jumpBufferTime;
        }

        public void ConsumeJumpBuffer()
        {
            _jumpBufferTimer = 0f;
        }
    }
}