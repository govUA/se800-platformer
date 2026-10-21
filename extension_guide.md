# 2D Platformer Framework — Extension Guide

Welcome to the Platformer Framework. This project is structured so you can implement new Player mechanics and Enemy
behaviours inside `Assets/Extensions/` without modifying any files in `Assets/Framework/`.

---

## 1. How to Add a New Player Mechanic

All player movement states inherit from `BasePlayerState` and run through the `PlayerController`.

### Step 1: Create your State class

Create a new script inside `Assets/Extensions/` (e.g., `GlideState.cs` or `DashState.cs`):

```csharp
using UnityEngine;
using Framework.Player;

namespace Extensions
{
    public class GlideState : BasePlayerState
    {
        public GlideState(PlayerContext context) : base(context)
        {
        }

        public override void Enter()
        {
            // Reduce gravity or set target glide velocity
            Context.Motor.SetGravityMultiplier(0.2f);
        }

        public override void Tick()
        {
            // Transition back to Grounded if we hit the floor
            if (Context.Sensors.IsGrounded)
            {
                Context.Controller.ChangeState(Context.Controller.GroundedState);
                return;
            } // Cancel glide on button release

            if (Context.Input.JumpReleased)
            {
                Context.Controller.ChangeState(Context.Controller.AirborneState);
                return;
            }
        }

        public override void FixedTick()
        {
            float targetSpeed = Context.Input.HorizontalInput * Context.Stats.walkSpeed;
            Context.Motor.MoveHorizontal(targetSpeed, Context.Stats.acceleration);
        }

        public override void Exit()
        {
            Context.Motor.SetGravityMultiplier(1f);
        }
    }
}
```

### Step 2: Hook up the State without modifying Core

Create an extension component in `Assets/Extensions/` and attach it to the `Player` GameObject:

```csharp
using UnityEngine;
using Framework.Player;

namespace Extensions
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAbilitiesExtension : MonoBehaviour
    {
        private PlayerController _controller;
        public GlideState GlideState { get; private set; }

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            GlideState = new GlideState(_controller.Context);
        }

        private void Update()
        {
            if (_controller.CurrentState == _controller.AirborneState && 
                _controller.Context.Input.JumpPressed &&
                _controller.Context.Motor.Velocity.y < 0f)
            {
                _controller.TryChangeState(GlideState);
            }
        }
    }
}
```

---

## 2. How to Add a New Enemy Behaviour

Enemy logic is decoupled through `IEnemyBehaviour` and initialized via `EnemyBehaviourProvider`.

### Step 1: Implement `IEnemyBehaviour`

Create a new behaviour script inside `Assets/Extensions/` (e.g., `JumpAttackBehaviour.cs`):

```csharp
using UnityEngine;
using Framework.Enemy;

namespace Extensions
{
    public class JumpAttackBehaviour : IEnemyBehaviour
    {
        private readonly Transform _playerTransform;
        private float _cooldownTimer;

        public JumpAttackBehaviour(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void Enter(BaseEnemy enemy)
        {
            _cooldownTimer = 1.5f;
        }

        public void Tick(BaseEnemy enemy)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f && enemy.Sensors.IsGrounded)
            {
                _cooldownTimer = 2f;
                if ((_playerTransform.position.x > enemy.transform.position.x && enemy.FacingDirection < 0) ||
                    (_playerTransform.position.x < enemy.transform.position.x && enemy.FacingDirection > 0))
                {
                    enemy.FlipDirection();
                }
                enemy.Motor.ApplyImpulse(new Vector2(enemy.FacingDirection * 6f, 10f));
            }
        }

        public void FixedTick(BaseEnemy enemy)
        {
        }

        public void Exit(BaseEnemy enemy)
        {
        }
    }
}
```

### Step 2: Create a Behaviour Provider Component

Create a provider component inheriting from `EnemyBehaviourProvider` so it can be attached to prefabs:

```csharp
using UnityEngine;
using Framework.Enemy;

namespace Extensions.Providers
{
    public class JumpAttackProvider : EnemyBehaviourProvider
    {
        [SerializeField] private Transform targetPlayer;

        public override IEnemyBehaviour CreateBehaviour(BaseEnemy enemy)
        {
            return new JumpAttackBehaviour(targetPlayer);
        }
    }
}
```

Attach this component directly to the `Enemy` GameObject alongside `BaseEnemy`.

---

## 3. Useful Core APIs

* **Sensors (`Context.Sensors` / `enemy.Sensors`)**:
    * `IsGrounded`: true when standing on ground.
    * `IsOnWall`: true when touching a wall.
    * `WallDirection`: `-1` (left wall), `1` (right wall), `0` (none).
* **Motor (`Context.Motor` / `enemy.Motor`)**:
    * `SetVelocityX(float x)`: Directly sets horizontal speed.
    * `SetVelocityY(float y)`: Directly sets vertical speed.
    * `ApplyImpulse(Vector2 force)`: Replaces linear velocity immediately.
    * `SetGravityMultiplier(float mult)`: Modifies downward pull.
* **Stats (`Context.Stats`)**:
    * ScriptableObject values for run speeds, jump apex parameters, and timings.
* **Controller State Transitions (`_controller`)**:
    * `TryChangeState(IState newState)`: Validates transition authorization before switching states.
    * `TryAirborneJump(float velocityMultiplier)`: Triggers aerial jump impulses and jump animations cleanly while
      airborne.

---

## 4. Connecting Custom Mechanics to the Animation System

All actor animations are driven through polymorphic hash contracts (`AnimationBoolHash`). Do not edit core animation
bridge files.

### Sustained Animation States (Booleans)

If your custom `BasePlayerState` or `IEnemyBehaviour` represents a sustained animation mode (such as sliding, dashing,
or chasing):

1. Define a private static hashed ID inside your state:
   ```csharp
   private static readonly int DashHash = Animator.StringToHash("IsDashing");
   ```
2. Override `AnimationBoolHash`:
   ```csharp
   public override int AnimationBoolHash => DashHash;
   ```

The animation bridge automatically listens to state transitions, clears previous flags, and sets the new boolean.

### One-Shot Triggers

For aerial jumps or one-shot animations, trigger them through the controller or bridge API:

```csharp
// Multi-jump triggers via PlayerController:
controller.TryAirborneJump(multiplier);

// Or direct trigger dispatch:
PlayerAnimationBridge bridge = GetComponentInChildren<PlayerAnimationBridge>();
bridge.NotifyJumpExecuted();
```