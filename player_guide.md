# Player Manual & Game Guide

A precision 2D platformer engineered around responsive aerial agility, timing forgiveness, and reactive enemy
behaviours.

---

## Controls

| Action                   | Key / Input                               | Notes                                                         |
|:-------------------------|:------------------------------------------|:--------------------------------------------------------------|
| **Move Left / Right**    | `A` / `D` or `Left Arrow` / `Right Arrow` | Run across surfaces and steer in the air.                     |
| **Jump**                 | `Space`                                   | Jump from the ground, leap off walls, or mid-air jump.        |
| **Variable Jump Height** | Hold vs. Tap `Space`                      | Tap for a short hop or hold through the apex for full height. |
| **Dash**                 | `Left Shift`                              | Propel horizontally in your current facing direction.         |

---

## Movement & Abilities

* **Precise Ground & Air Control**: Swift acceleration and sharp deceleration ensure quick turnarounds on platforms,
  with responsive mid-air steering to adjust jumps.
* **Dynamic Fall Physics**: Early release of the jump button increases gravity to cut your jump short, while standard
  downward momentum naturally increases fall speed for crisp landing feedback.
* **Jump Buffering**: Pressing jump slightly before touching the ground buffers your input, automatically triggering a
  jump the instant you land.
* **Coyote Time**: Walking off an edge grants a brief grace window where you can still jump without falling into a void.
* **Wall Slide & Wall Jump**:
    * Touching a wall on descent locks you into a steady, controlled wall slide.
    * Press `Space` against or sliding down a wall to perform a wall kick, boosting outward and upward with momentary
      directional control lock.
    * Push away from the wall to cleanly disengage into freefall.
* **Double Jump**: While airborne and clear of the initial coyote window, press `Space` to execute an extra jump. Your
  available extra jumps recharge instantly upon touching solid ground.
* **Air Dash**: Tap `Left Shift` to dash forward at fixed horizontal velocity with zero gravity interference for a
  fraction of a second. This ability has a short cooldown between activations.

---

## Enemy Types & Hazards

* **Patroller**: Steadily paces across patrol routes and automatically turns around upon hitting walls or encountering
  platform ledges.
* **Chaser**: Patrols normally until you enter its detection proximity, at which point it accelerates and relentlessly
  chases you toward platform boundaries.
* **Leaper (Jumper)**: Detects your position and leaps diagonally through the air toward you, entering a brief ground
  cooldown before it can jump again.