# 2D Precision Platformer Project

A responsive, modular 2D platformer template developed in Unity. The project combines game feel techniques (variable
jumps, coyote time, input buffering) with a decoupled state-machine architecture that is easy to extend.

## Table of Contents

- [Documentation](#documentation)
- [Project Architecture](#project-architecture)
- [Assignments & Development Releases](#assignments--development-releases)
    - [Week 1 — Movement Release](#week-1--movement-release)
        - [Team Role Breakdown](#team-role-breakdown-week-1)
        - [Git Discipline & Integration Workflow](#git-discipline--integration-workflow)
    - [Week 2 — Animation & Feedback Release](#week-2--animation--feedback-release)
        - [Architecture Overview](#architecture-overview-assetsframeworkanimation)
        - [Team Role Breakdown & Deliverables](#team-role-breakdown--deliverables-week-2)
        - [Verification & Testing Controls](#verification--testing-controls)

---

## Documentation

Depending on what you are looking for, refer to the following documentation guides:

* [**Player Manual & Game Guide**](player_guide.md)  
  A gameplay and mechanics overview for players and level designers. Covers primary movement controls, diagnostic
  reaction triggers, advanced mobility (dash, double-jump, wall-sliding), and enemy behaviors.
* [**Framework Extension Guide**](extension_guide.md)  
  A developer guide detailing how to build custom player abilities, enemy AI behaviors, and connect them to the
  animation bridge inside `Assets/Extensions/` without altering core framework code.
* [**Animation & Feedback Architecture Guide**](animation_guide.md)  
  A technical guide detailing the decoupled bridge layer, zero-allocation pre-hashed parameters (`AnimationHashes`),
  Animator parameter contracts, and non-combat diagnostic event dispatching.

---

## Project Architecture

```
Assets/
├── Framework/                  # Immutable core framework
│   ├── Animation/              # Decoupled bridges, event receivers & test rig
│   │   ├── ActorAnimationBridge.cs
│   │   ├── PlayerAnimationBridge.cs
│   │   ├── EnemyAnimationBridge.cs
│   │   ├── AnimationHashes.cs
│   │   ├── AnimationEventReceiver.cs
│   │   └── AnimationTestRig.cs
│   ├── Core/
│   │   ├── Input/              # Input provider interfaces & bindings
│   │   ├── Motor/              # 2D physics and custom gravity execution
│   │   ├── Sensors/            # Ground, ledge, and wall detection
│   │   └── StateMachine/       # Base finite state machine interfaces
│   ├── Data/                   # Movement tuning ScriptableObjects
│   ├── Enemy/                  # Base enemy controller & stock behaviors
│   └── Player/                 # Core player controller & basic states
├── Extensions/                 # Custom gameplay features & modular abilities
├── DashAbility.cs          # Horizontal impulse dash
├── DashState.cs            # Dash state implementation
├── DoubleJumpAbility.cs    # Air jump mechanic
├── ChaseBehaviour.cs       # Proximity-based enemy aggression
├── JumpToPlayerBehaviour.cs# Predictive enemy leap attack
└── EnemyBehaviourInitializer.cs # Runtime behaviour configuration
```

---

## Assignments & Development Releases

### Week 1 — Movement Release

Developed within
[**Unity Product Lab (Week 1 — Movement Release)**](https://teaching.kse.org.ua/mod/assign/view.php?id=135128)
by a two-person team operating under the [**Platformer Framework Architect**](https://github.com/govUA) +
[**AI-assisted Framework Developer**](https://github.com/KristinaRiabova304) role distribution:

#### Team Role Breakdown (Week 1)

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

#### Git Discipline & Integration Workflow

* **Separation of Concerns**: Complete decoupling between `Assets/Framework/` (immutable core) and
  `Assets/Extensions/` (modular mechanics), verifying that extension points work as intended.
* **Feature Branches & Pull Requests**: Development was partitioned into logical feature branches with discrete PRs
  submitted for the core state machine, motor physics, abilities, and enemy behaviors.
* **Movement Release Validation**: Tuned control responsiveness and mechanics transitions in a dedicated test scene to
  ensure the vertical slice satisfies Week 1 gameplay and stability requirements.

---

### Week 2 — Animation & Feedback Release

Following the Week 1 movement foundation, the project was expanded to deliver an animated and reactive playable vertical
slice according to the **Platformer Framework Architect + AI-assisted Framework Developer** assignment workflow.

#### Architecture Overview (`Assets/Framework/Animation/`)

To keep animation logic strictly separated from core physics and state logic, an event- and state-driven bridge
architecture was established:

* **`ActorAnimationBridge`**: Base component connecting `ActorMotor2D` and `EnvironmentDetector2D` to Unity's `Animator`
  without coupling gameplay states to clip names.
* **`PlayerAnimationBridge`**: Subscribes to `PlayerController.OnStateChanged` and frame-by-frame motor parameters (
  `Speed`, `VerticalVelocity`, `IsGrounded`, `IsWallSliding`, `IsDashing`) to drive Animator parameters dynamically.
* **`EnemyAnimationBridge`**: Updates enemy visual locomotion (`Speed`, `IsGrounded`, `IsChasing`) based on active enemy
  sensor and motor states.
* **`AnimationHashes`**: Static pre-hashed integer cache (`Animator.StringToHash`) ensuring zero-allocation string
  lookups during runtime updates.
* **`AnimationEventReceiver`**: Lightweight listener attached to animated GameObjects to receive frame-accurate
  interaction triggers via `OnInteractionEvent(string actionName)` and dispatch diagnostic logs without coupling to
  combat or damage systems.
* **`AnimationTestRig`**: Diagnostic harness allowing keyboard triggers for reaction testing (`H` for Hurt, `K` for
  Death, `J` for Action/Interaction).

#### Team Role Breakdown & Deliverables (Week 2)

##### Platformer Framework Architect

* **Animation Framework API**: Designed and implemented the decoupled bridge layer (`ActorAnimationBridge`,
  `PlayerAnimationBridge`, `EnemyAnimationBridge`, `AnimationHashes`, `AnimationEventReceiver`).
* **Core Clips & Integration**: Authored and integrated baseline locomotion Animator controllers and clips (Player
  Run/Idle blending, Enemy Patrol).
* **Decoupled Event Pipelines**: Ensured interaction actions, reaction triggers (`Hurt`, `Death`), and extension
  states (`IsDashing`, `JumpTrigger`) expose clean Inspector/API hooks without modifying immutable gameplay physics.
* **PR Code Reviews & API Iteration**: Evaluated upstream integration PRs from the AI-assisted developer, refining
  bridge event timing and state transition guarantees (e.g., auto-clearing `IsDashing` on Hurt/Death).

##### AI-Assisted Framework Developer

* **API Consumption via AI**: Used AI-assisted analysis to parse bridge events, state machine hooks, and parameter
  mappings.
* **Extension Animations & States**:
    * Bound custom ability states (`DashState`, `DoubleJumpAbility`) to the animation pipeline via `IsDashing` and
      `NotifyJumpExecuted()`.
    * Hooked custom enemy AI (`ChaseBehaviour`, `JumpToPlayerBehaviour`) into `EnemyAnimationBridge.SetChasing()` and
      jumping triggers.
* **Reaction & Interaction States**:
    * Connected Hurt and Death reaction transitions on Player and Enemy test models.
    * Authored an interaction/action state (`Action` trigger) with an embedded Unity `AnimationEvent` invoking
      `AnimationEventReceiver.OnInteractionEvent`, logging interactions cleanly without health or combat dependencies.
* **Presentation & Polish**:
    * Added movement/action feedback (particle dust on dashes/landings and animated environment interaction markers).
    * Submitted targeted Pull Requests across feature branches for review.

#### Verification & Testing Controls

| Key          | Target        | Triggered Action    | Expected Output                                                                                                                                          |
|:-------------|:--------------|:--------------------|:---------------------------------------------------------------------------------------------------------------------------------------------------------|
| `H`          | Active Bridge | Hurt Trigger        | Character plays Hurt animation reaction; resets active Dash flags.                                                                                       |
| `K`          | Active Bridge | Death Trigger       | Character enters Death state; locomotion and state flags clear.                                                                                          |
| `J`          | Active Bridge | Interaction Trigger | Executes action clip; fires `AnimationEventReceiver.OnInteractionEvent` printing `[AnimationEvent] Interaction dispatched: <ActionName>` to the Console. |
| `Left Shift` | Player        | Dash Ability        | Triggers `DashState` physics and sets `IsDashing = true` on the Animator.                                                                                |