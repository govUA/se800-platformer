# 2D Precision Platformer Project

A responsive, modular 2D platformer template developed in Unity. The project combines game feel techniques (variable
jumps, coyote time, input buffering) with a decoupled state-machine architecture that is easy to extend.

## Documentation

Depending on what you are looking for, refer to the following documentation:

* [**Player Manual & Game Guide**](player_guide.md)  
  A gameplay and mechanics overview for players and level designers. Covers primary movement controls, advanced
  mobility (dash, double-jump, wall-sliding), and enemy behaviors.
* [**Framework Extension Guide**](extension_guide.md)  
  A developer guide detailing how to build custom player abilities and enemy AI behaviors inside `Assets/Extensions/`
  without altering core framework code.

## Project Architecture

```
Assets/
├── Framework/                  # Immutable core framework
│   ├── Core/
│   │   ├── Input/              # Input provider interfaces & bindings
│   │   ├── Motor/              # 2D physics and custom gravity execution
│   │   ├── Sensors/            # Ground, ledge, and wall detection
│   │   └── StateMachine/       # Base finite state machine interfaces
│   ├── Data/                   # Movement tuning ScriptableObjects
│   ├── Enemy/                  # Base enemy controller & stock behaviors
│   └── Player/                 # Core player controller & basic states
└── Extensions/                 # Custom gameplay features & modular abilities
    ├── DashAbility.cs          # Horizontal impulse dash
    ├── DoubleJumpAbility.cs    # Air jump mechanic
    ├── ChaseBehaviour.cs       # Proximity-based enemy aggression
    └── JumpToPlayerBehaviour.cs# Predictive enemy leap attack
```

## Assignment Progress & Collaboration Workflow

This project was developed within [**Unity Product Lab (Week 1 — Movement Release)**](https://teaching.kse.org.ua/mod/assign/view.php?id=135128) by a two-person team operating under
the [**Platformer Framework Architect**](https://github.com/govUA) + [**AI-assisted Framework Developer**](https://github.com/KristinaRiabova304) role distribution:

### Team Role Breakdown

* **Platformer Framework Architect**:
    * Designed the core modular architecture inside `Assets/Framework/` around a decoupled Finite State Machine (
      `IState`, `BasePlayerState`).
    * Built custom motor physics (`ActorMotor2D`) with formula-driven jump kinematics and dynamic gravity scaling (
      variable jump height, fast-fall multipliers).
    * Implemented environmental query sensors (`EnvironmentDetector2D`) for ledge, ground, and wall detection.
    * Set up Inspector-driven configuration through `MovementStatsSO` to fine-tune game feel variables (coyote time,
      jump buffer, air control, wall kick impulses).
    * Exposed clean extension points for custom abilities and enemy behaviors without requiring edits to core systems.

* **AI-assisted Framework Developer**:
    * Consumed the framework as a downstream user via the public API and extension interfaces.
    * Leveraged AI tooling for architecture comprehension, interface matching, and rapid iteration.
    * Implemented new Player abilities inside `Assets/Extensions/`:
        * `DashAbility` & `DashState`: Horizontal dash impulse with temporary gravity suspension and cooldown tracking.
        * `DoubleJumpAbility`: Multi-jump tracking integrated cleanly with airborne state transitions.
    * Implemented reactive Enemy AI behaviors via `IEnemyBehaviour`:
        * `ChaseBehaviour`: Proximity detection and pursuit while respecting platform borders and ledges.
        * `JumpToPlayerBehaviour`: Grounded distance checks and timed leap impulses aimed at the player.

### Git Discipline & Integration Workflow

* **Separation of Concerns**: Complete decoupling between `Assets/Framework/` (immutable core) and
  `Assets/Extensions/` (modular mechanics), verifying that extension points work as intended.
* **Feature Branches & Pull Requests**: Development was partitioned into logical feature branches with discrete PRs
  submitted for the core state machine, motor physics, abilities, and enemy behaviors.
* **Movement Release Validation**: Tuned control responsiveness and mechanics transitions in a dedicated test scene to
  ensure the vertical slice satisfies Week 1 gameplay and stability requirements.