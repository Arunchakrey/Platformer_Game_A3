# **Sustainable Development Goals (SDG) Platformer Game**

## Overview

This project is a 2D/3D hybrid platformer game developed in Unity as part of a group assignment. The game explores sustainable development themes through interactive levels that test player reflexes and decision-making while maintaining an engaging visual and audio experience.

The project demonstrates our understanding of Unity’s physics, UI systems, sound design, and game logic, implemented collaboratively through version control on GitHub.

---

## Game Link

---

## 🕹️ Game Controls

| Action              | Key                       |
| ------------------- | ------------------------- |
| Move Left           | `A`                       |
| Move Right          | `D`                       |
| Jump / Swim         | `Spacebar`                |
| Pause / Resume      | `Esc`                     |
| Interact (Planting) | `P`                       |
| Adjust Volume       | Slider (Top-Right Corner) |

---

## Resetting Progress

To reset your progress:

- Delete the PlayerPrefs data from your local build by navigating to:
  ```
  C:\Users\<YourUsername>\AppData\LocalLow\<CompanyName>\<GameName>
  ```

---

## Team Contributions

### **Arunchakrey**

**Contributions:**

- Implemented **progress bar** for 2D levels.
- Added **SoundFX** for player actions and integrated a **volume slider** in the UI.
- Designed **camera shake** feedback when the player takes damage (land only).
- Created **Planting Spot prefab** for the 3D level.

**References:**

- Kenney. (2019). _Impact Sounds_ [Audio asset]. Kenney.nl. https://kenney.nl/assets/impact-sounds
- Kenney. (2020). _Sci-fi Sounds_ [Audio asset]. Kenney.nl. https://kenney.nl/assets/sci-fi-sounds
- Kenney. (2025a). _Digital Audio_ [Audio asset]. Kenney.nl. https://kenney.nl/assets/digital-audio
- Kenney. (2025b). _Music Jingles_ [Audio asset]. Kenney.nl. https://kenney.nl/assets/music-jingles

---

### **Aaryan**

**Contributions:**

- Designed and built the **3D level and map layout** using the Poly Style Platformer pack.
- Created **3D character** with animations using free character assets.
- Handled **colliders** for all in-game elements.
- Developed **custom camera follow script** with adjustable distance.
- Managed **sprite setup** for 3D characters.
- Implemented **health, sound, and side-scroll systems**.
- Created **SDG dialogue system** at start and end of levels.
- Added **spike damage** with revised script from 2D version.
- Added **invisible walls** and **kill zone** mechanics.
- Implemented **respawn system** upon player death.

**References:**

- Supercyan. (n.d.). _Character Pack Free Sample_ [Unity asset]. Unity Asset Store. https://assetstore.unity.com/packages/3d/characters/humanoids/character-pack-free-sample-79870
- JustCreate. (n.d.). _POLY STYLE – Platformer Starter Pack_ [Unity asset]. Unity Asset Store. https://assetstore.unity.com/packages/3d/environments/poly-style-platformer-starter-pack-284167

---

### **Ratanakvisal**

**Contributions:**

- Resolved major and minor **bug fixes** across scripts and prefabs.
- Handled **UI resizing** and scaling issues for build compatibility.
- Managed **Git repository organization** and branch merging.
- Developed **3D main menu interface** and background design.

**References:**

- JustCreate Studio. (n.d.). _Low Poly Simple Nature Pack_ [Unity asset]. Unity Asset Store. https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153

### **Tristan**

**Contributions:**

- Designed and built Level 4 from scratch with log collection and house building mechanics.
- Created collectible log system with auto-pickup functionality and visual feedback.
- Implemented falling brick hazards with damage and physics-based knockback.
  Developed player inventory system allowing carrying up to 2 logs at a time with visual stacking.
- Created build site with progress tracking and construction stages.
  Implemented exit door trigger that opens upon level completion.
- Developed comprehensive difficulty system (Easy/Normal/Hard) affecting player health, enemy speed, and hazard damage.
- Implemented data persistence using PlayerPrefs and DontDestroyOnLoad patterns.
- Ensured difficulty settings persist across all levels using scene management callbacks.
- Created HealthUIConnector to bridge Health component with UI system.
  Fixed compilation errors by updating deprecated Unity APIs (FindObjectOfType, Rigidbody2D.isKinematic).
- Made SoundEffectManager accessible and added PlaySound functionality for audio integration.
- Implemented data-driven architecture using ScriptableObjects for maintainability.

---

## Completed Requirements

- [x] Functional 2D and 3D playable levels
- [x] Interactive UI (Pause, Game Over, Volume Slider, Progress Bar)
- [x] Audio feedback for player actions
- [x] Dialogue and narrative integration
- [x] Respawn and death mechanics
- [x] Polished 3D environment with SDG theme

---

## Known Issues / Unresolved

- Camera shake currently only works on land surfaces.
- UI panels may scale differently depending on resolution in builds.
- Cursor visibility occasionally fails on scene transitions.
- 3D level doesn't reset health when jumping off map
- 3D menu title wrong position in final web build.
- Dialogue remains in GitHub and not in the final Web build.
- 3D movement drifts.

---

## Credits

All external assets used are open-source or free assets, cited under APA 7th edition guidelines.  
Game developed by **Arunchakrey**, **Aaryan**, **Tristan** and **Ratanakvisal** for the _Game Development Lifecycle_ course at _Swinburne University of Technology_.

---

© 2025 Platformer Game Team. All rights reserved.
