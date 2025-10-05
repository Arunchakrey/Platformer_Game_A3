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
- **Arunchakrey Puthyrith (@Arunchakrey)** - All mechanics of player on Land, including walking and jumping physics, land enemies "AI" (grasshopper and bear) and damage to the player, spikes and springs scripts and interaciton with player, UI for health, pickup script, speed boost script with cooldown, visual FX on player when speed boost is active, tilemap setup for level 1, camera following player character.
- **Aaryan** - Made Level 3, used a variety of sprites and backgrounds, made modifications to health UI system.
- **Tristen Lee (@TristT1)** – The build site system, house construction logic, log gathering, and other gameplay elements for Level 4 (Build Houses) were completely designed and developed. built scripts for MaterialPickup, CarryStack, and BuildSite that integrate scores into the global HUD. Falling Brick hazard with damage and knockback, exit door system unlocking upon build completion, and complete HUD reconstruction (Score, Total, and Build Progress) were included. In addition, new sprites (log, brick, little log cabin, and exit door) were made and integrated, the background was designed using the Free Pixel Art Forest Pack, and merge resolution and level polish were managed.
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
- Some sprites are not used as prefabs
- Unoptimized game
- Unorganized scripts
- On gameover screen, mouse button might disappear (not sure why this happens, works okay in unity editor).

### F) Third-Party Assets & Credits

- Underwater Sprites: ChatGPT generated.
- Menu buttons: "Menu Buttons" - https://nectanebo.itch.io/menu-buttons
- ForestTile "Tile Map" & Enemy Sprites: Sunny Land - https://assetstore.unity.com/packages/2d/characters/sunny-land-103349
- Background Sprite "level 1" - https://assetstore.unity.com/packages/2d/environments/pixel-art-woods-tileset-and-background-280066
- Land Character Sprite: Super Grotto Escape - https://assetstore.unity.com/packages/2d/environments/super-grotto-escape-pack-238393
- Sapling: ChatGPT generated
- Spring sprite: drawn by Arunchakrey Puthyrith
- Spike sprite: drawn by Arunchakrey Puthyrith
- Background, foreground and ground sprites "level3 - https://dreamir.itch.io/tileset-wasteland
- Food sprites "level3" - https://ghostpixxells.itch.io/pixel-mart
- Log, Brick, House, and Exit Door Sprites: Custom AI-generated assets (ChatGPT Generated)
