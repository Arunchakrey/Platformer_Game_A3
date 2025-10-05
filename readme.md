# Platformer_Game — Unity 2D (Team of 4)

A 2D Unity 6 project for **Assessment 2**. The game features multiple levels, clear game states (Main Menu → Level Select → Gameplay → Pause → Game Over / Win), data-driven tuning, physics, pickups, oxygen + health systems, and a WebGL build.

---

## How to Run

### A) Unity Editor

1. **Unity version:** Unity **6** (2025.x).
2. Open the project in Unity Hub.
3. Open **`Scenes/MainMenu.unity`** to jump in.
4. Press **Play**.

### B) WebGL

Playable build link: **itch.io**

---

## Controls

| Action                    | Keyboard             |
| ------------------------- | -------------------- |
| Move / Swimming Direction | **A/D** or **←/→**   |
| Jump / Swim Stroke        | **Space**            |
| Interact / Collect        | Touch item (trigger) |
| Pause / Resume            | **Esc**              |
| (Debug) Force Win         | **N**                |
| (Debug) Force Game Over   | **K**                |

> **Level 2 (swimming):** Aim with WASD/Arrows, press **Space** to perform a swim **stroke**. Oxygen drains underwater; surface to refill.

---

### C) Team & Contributions

- **Ratanakvisal Heng (@hengr.visal)** - All Underwater mechanisms (Level2) including, swim physics, drown damage over time, bubble particles. As well as UI menu of the game, including level progressions, pause screen, and game over screen.
- _All teammates contributed to playtesting, code reviews, prefab/scene setup, and final polish._

### D) Requirements Checklist

#### **Baseline**

- **Game States**: Main Menu -> Level Select -> Gameplay -> Pause -> Game Over/Win.
- **Levels**: At least 3 short levels.
- **Points & HUD**: Score displayred; pickups award points
- **Physics & Collisions**: Correct Layer Collision Matrix; multiple Physics Matericals in use.
- **Modular Systems**: Health & Damage, Pickup system, Timers & Cooldowns.
- **Assets mix**: 1 team created asset and multiple third part asset.

### E) Known Issues / Limitations

- Missing Audio/SFX
- Colliders are not fine tuned, leading to inaccurate damage taken
-

### F) Third-Party Assets & Credits

- Underwater Sprites: ChatGPT generated.
- Menu buttons: "Menu Buttons" - https://nectanebo.itch.io/menu-buttons
-
