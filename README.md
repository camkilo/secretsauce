# Arena Survival Game

A third-person 3D arena survival game built in Unity, set in a circular stone chamber with atmospheric lighting and challenging wave-based combat.

## 🎮 Game Features

### Player Mechanics
- **Responsive Movement**: Smooth WASD character control with third-person camera
- **Dodge Roll**: Space bar for dodge with brief invulnerability and stamina cost
- **Combat System**: 
  - Light Attack (Left Click): Fast, low-damage melee
  - Heavy Attack (Right Click): Slow, high-damage melee
- **Stamina Management**: Resource system for dodges and attacks with auto-regeneration

### Enemy Types
- **Rushers** 🏃: Fast, aggressive melee attackers with low health
- **Brutes** 💪: Slow, tanky enemies with devastating melee damage
- **Casters** 🔮: Fragile ranged enemies that maintain distance

### Environment
- **Circular Stone Arena**: Enclosed combat chamber with stone walls
- **Pillars**: 8 stone pillars providing tactical cover
- **Atmospheric Lighting**: 12 flickering torches creating cinematic ambiance
- **Fog Effects**: Dense fog for immersive atmosphere

### UI
- Health Bar (Red)
- Stamina Bar (Green)
- Survival Timer
- Wave Counter
- Game Over Screen

## 🚀 Quick Start

### Prerequisites
- Unity 2022.3.10f1 or later
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd secretsauce
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Add" and select the cloned folder
   - Open with Unity 2022.3.10f1 or compatible version

3. **Setup the Scene**
   - Open `Assets/Scenes/MainArena.unity`
   - Follow the detailed setup instructions in `DOCUMENTATION.md`

4. **Play**
   - Press Play in Unity Editor
   - Survive as long as possible!

## 🎯 Controls

| Input | Action |
|-------|--------|
| WASD | Move character |
| Mouse | Rotate camera |
| Space | Dodge roll |
| Left Click | Light attack |
| Right Click | Heavy attack |
| ESC | Pause/Resume |
| R | Restart (after death) |

## 📁 Project Structure

```
Assets/
├── Scenes/          # Game scenes
├── Scripts/         # C# gameplay scripts
├── Prefabs/         # Reusable game objects
└── Materials/       # Visual materials
```

## 🔧 Key Scripts

- **PlayerController.cs**: Player movement, combat, and stats
- **EnemyController.cs**: AI behavior for all enemy types
- **WaveSpawner.cs**: Progressive wave-based enemy spawning
- **GameManager.cs**: Game state and flow control
- **ArenaBuilder.cs**: Procedural arena generation
- **CameraController.cs**: Third-person camera system

## 📖 Documentation

For detailed setup instructions, customization guide, and technical documentation, see [DOCUMENTATION.md](DOCUMENTATION.md).

## 🎨 Visual Design

- **Art Style**: Realistic stone textures and materials
- **Lighting**: Cinematic torch lighting with soft shadows
- **Atmosphere**: Dense fog and ambient lighting
- **Effects**: Flickering torch lights for atmosphere

## 🏆 Gameplay Goal

Survive as long as possible against increasingly difficult waves of enemies. Test your combat skills, stamina management, and tactical positioning!

## 🛠️ Built With

- **Engine**: Unity 2022.3.10f1
- **Language**: C#
- **AI**: Unity NavMesh system
- **UI**: Unity UI with TextMeshPro

## 📝 Notes

- This is a foundation implementation with core mechanics in place
- Animations require an Animator Controller setup in Unity
- Audio and advanced VFX are not included in base implementation
- Designed for PC with mouse and keyboard controls

## 🤝 Contributing

This project is part of a game development exercise. For contributions or suggestions, please open an issue or pull request.

## 📄 License

See LICENSE file for details.
