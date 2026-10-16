# Animation & Feedback Architecture Guide

This document specifies the architecture, Animator parameter conventions, and event pipelines introduced in **Week 2 (
Animation & Feedback Release)**.

---

## 1. Architecture & Design Principles

* **Decoupled Presentation**: Gameplay mechanics, sensors, and state machines do not depend on Unity's `Animator`. All
  animation communication flows outward via `PlayerAnimationBridge` and `EnemyAnimationBridge`.
* **Zero Allocation Hashes**: Animator property names are hashed once at startup via `AnimationHashes.cs` using
  `Animator.StringToHash()`.
* **Diagnostic Event Decoupling**: Interaction, Hurt, and Death states are fully testable without requiring health
  pools, hitboxes, or combat resolution.

---

## 2. Animator Parameters Contract

Any Animator Controller assigned to a Player or Enemy prefab must configure the following standardized parameters:

### Shared Locomotion & Reaction Parameters

| Parameter Name     | Type    | Purpose / Trigger Condition                                          |
|:-------------------|:--------|:---------------------------------------------------------------------|
| `Speed`            | Float   | Absolute horizontal linear velocity (`Mathf.Abs(motor.Velocity.x)`). |
| `VerticalVelocity` | Float   | Y-axis motor velocity (`motor.Velocity.y`).                          |
| `IsGrounded`       | Bool    | Sensor check: `EnvironmentDetector2D.IsGrounded`.                    |
| `IsWallSliding`    | Bool    | Active while player is in `WallSlideState`.                          |
| `Hurt`             | Trigger | Triggered to play hit-stun / damage-reaction clip.                   |
| `Death`            | Trigger | Triggered upon actor defeat. Resets continuous state booleans.       |
| `Action`           | Trigger | Generic interaction/attack animation trigger.                        |

### Ability & AI Extension Parameters

| Parameter Name | Type    | Purpose / Trigger Condition                                 |
|:---------------|:--------|:------------------------------------------------------------|
| `IsDashing`    | Bool    | Active during `DashState`; resets upon completion or Hurt.  |
| `Jump`         | Trigger | Dispatched on ground jump or `DoubleJumpAbility` execution. |
| `IsChasing`    | Bool    | Active when enemy enters detection radius of target.        |

---

## 3. Animation Events & Diagnostics

### Diagnostic Verification Keys (`AnimationTestRig`)

* `H`: Dispatches `TriggerHurt()` on the bound bridge component.
* `K`: Dispatches `TriggerDeath()` on the bound bridge component.
* `J`: Dispatches `TriggerAction()` to trigger the interaction animation.

### Interaction Verification via `AnimationEventReceiver`

Action animations place a standard Unity `AnimationEvent` at the impact/completion keyframe:

* **Function**: `OnInteractionEvent(string actionName)`
* **Behavior**: Logs `[AnimationEvent] Interaction dispatched: <actionName> at timestamp <Time>s` to the Unity console.