using System.Collections;
using UnityEngine;
using Framework.Player;
using Framework.Animation;
using Framework.Core.StateMachine;

namespace Extensions
{
    public class PlayerDeadState : BasePlayerState
    {
        public override bool CanTransitionTo(IState nextState) => false; // Locked out completely

        public PlayerDeadState(PlayerContext context) : base(context)
        {
        }

        public override void Enter()
        {
            Context.Motor.SetGravityMultiplier(1f);
        }

        public override void FixedTick()
        {
            Context.Motor.MoveHorizontal(0f, Context.Stats.deceleration);
        }
    }

    [RequireComponent(typeof(PlayerController))]
    public class PlayerDeathReceiver : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private float gameOverDelay = 1.2f;

        private PlayerController _playerController;
        private PlayerAnimationBridge _animationBridge;
        private bool _isDead;

        public bool IsDead => _isDead;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _animationBridge = GetComponentInChildren<PlayerAnimationBridge>();
        }

        /// <summary>
        /// Kills the player. Set skipHurt to true for instant death animation.
        /// </summary>
        public void KillAndKnockback(Vector2 knockbackImpulse, bool skipHurt = false)
        {
            if (_isDead) return;
            _isDead = true;

            // 1. Shift player into un-cancellable dead state
            _playerController.ChangeState(new PlayerDeadState(_playerController.Context));

            // 2. Apply physics impulse directly via motor
            _playerController.Context.Motor.ApplyImpulse(knockbackImpulse);

            // 3. Trigger animations
            if (_animationBridge != null)
            {
                if (skipHurt)
                {
                    // Directly triggers Player_Death from AnyState
                    _animationBridge.TriggerDeath();
                }
                else
                {
                    // Triggers Hurt, which will automatically roll into Death via Exit Time
                    _animationBridge.TriggerHurt();
                }
            }

            // 4. Trigger Game Over
            StartCoroutine(GameOverSequence(skipHurt ? 0.8f : gameOverDelay));
        }

        private IEnumerator GameOverSequence(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}