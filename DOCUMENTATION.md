# Arena Survival Game - Technical Documentation

## Overview
A third-person 3D arena survival game built in Unity. Players fight waves of enemies in a circular stone chamber with cinematic lighting and atmospheric effects.

## Game Features

### Player Mechanics
- **Movement**: WASD keys for responsive character movement
- **Camera Control**: Mouse to control third-person camera view
- **Dodge Roll**: Space bar for dodge roll with brief invulnerability and stamina cost
- **Light Attack**: Left mouse button for quick melee attacks
- **Heavy Attack**: Right mouse button for powerful melee attacks
- **Stamina System**: Regenerating stamina resource for dodge rolls and attacks

### Enemy Types
1. **Rushers** (Fast enemies)
   - High speed, low health
   - Aggressive melee attacks
   - 50% spawn rate

2. **Brutes** (Heavy enemies)
   - Slow movement, high health
   - Devastating melee damage
   - Larger size (1.5x scale)
   - 30% spawn rate

3. **Casters** (Ranged enemies)
   - Medium speed, fragile
   - Ranged projectile attacks
   - Maintains distance from player
   - 20% spawn rate

### Environment
- **Circular Arena**: Stone chamber with walls and floor
- **Pillars**: 8 stone pillars providing cover
- **Torches**: 12 flickering torches for lighting
- **Fog**: Atmospheric fog for cinematic effect
- **Dynamic Lighting**: Soft shadows and cinematic lighting setup

### UI Elements
- **Health Bar**: Red bar showing player health
- **Stamina Bar**: Green bar showing stamina level
- **Survival Timer**: Real-time survival duration display
- **Wave Counter**: Current wave number
- **Game Over Screen**: Final survival time display

### Game Systems
- **Wave Spawner**: Progressive difficulty with increasing enemy counts
- **Combat System**: Melee and ranged damage calculation
- **AI System**: NavMesh-based enemy pathfinding
- **Game Manager**: Game state management and flow control

## File Structure

```
Assets/
├── Scenes/
│   └── MainArena.unity          # Main game scene
├── Scripts/
│   ├── PlayerController.cs      # Player movement and combat
│   ├── EnemyController.cs       # Enemy AI and behavior
│   ├── Projectile.cs            # Ranged attack projectiles
│   ├── WaveSpawner.cs           # Enemy wave management
│   ├── GameManager.cs           # Game state controller
│   ├── GameUIController.cs      # UI management
│   ├── CameraController.cs      # Third-person camera
│   ├── ArenaBuilder.cs          # Procedural arena generation
│   └── TorchFlicker.cs          # Torch light effects
├── Prefabs/                     # (To be populated)
└── Materials/                   # (To be populated)
```

## Setup Instructions

### Prerequisites
- Unity 2022.3.10f1 or later
- Basic understanding of Unity Editor

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd secretsauce
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Add" and select the secretsauce folder
   - Open the project with Unity 2022.3.10f1 or compatible version

3. **Scene Setup**
   - Open `Assets/Scenes/MainArena.unity`
   
4. **Create Player GameObject**
   - Create Empty GameObject named "Player"
   - Tag it as "Player"
   - Add CharacterController component
   - Add PlayerController script
   - Position at (0, 1, 0)

5. **Create Camera**
   - Create Main Camera (if not exists)
   - Tag it as "MainCamera"
   - Add CameraController script
   - Set Player as target
   - Position at (0, 5, -7)

6. **Create Arena**
   - Create Empty GameObject named "Arena"
   - Add ArenaBuilder script
   - Configure radius and pillar settings

7. **Create Enemy Prefab**
   - Create Capsule GameObject
   - Tag it as "Enemy"
   - Add NavMeshAgent component
   - Add EnemyController script
   - Save as Prefab in Assets/Prefabs/

8. **Create Spawner**
   - Create Empty GameObject named "WaveSpawner"
   - Add WaveSpawner script
   - Assign enemy prefab
   - Set arena center reference

9. **Create Game Manager**
   - Create Empty GameObject named "GameManager"
   - Add GameManager script

10. **Setup UI Canvas**
    - Create UI Canvas
    - Add health bar (Image with Fill)
    - Add stamina bar (Image with Fill)
    - Add timer text (TextMeshPro)
    - Add wave text (TextMeshPro)
    - Create game over panel
    - Add GameUIController script to Canvas

11. **Bake NavMesh**
    - Select Arena floor
    - Mark as "Navigation Static"
    - Window → AI → Navigation
    - Click "Bake"

12. **Configure Input**
    - Edit → Project Settings → Input Manager
    - Verify default axes (Horizontal, Vertical, Mouse X, Mouse Y)

## Controls

- **WASD**: Move character
- **Mouse**: Rotate camera
- **Space**: Dodge roll
- **Left Click**: Light attack
- **Right Click**: Heavy attack
- **ESC**: Pause/Resume
- **R**: Restart (after death)

## Technical Details

### Player Stats
- Health: 100 HP
- Stamina: 100 SP
- Move Speed: 5 units/sec
- Dodge Speed: 12 units/sec
- Light Attack: 15 damage, 10 stamina cost
- Heavy Attack: 35 damage, 30 stamina cost
- Dodge Cost: 25 stamina
- Stamina Regen: 15 SP/sec

### Enemy Stats

**Rusher:**
- Health: 30 HP
- Speed: 6 units/sec
- Damage: 8
- Range: 1.5 units

**Brute:**
- Health: 100 HP
- Speed: 2 units/sec
- Damage: 25
- Range: 2.5 units

**Caster:**
- Health: 20 HP
- Speed: 3 units/sec
- Damage: 12 (projectile)
- Range: 8 units

### Wave System
- Base enemies per wave: 3
- Difficulty scaling: 1.2x per wave
- Max enemies per wave: 20
- Time between waves: 10 seconds

## Customization

### Modifying Player Stats
Edit values in PlayerController component:
- Move Speed
- Attack Damage
- Stamina costs
- Health/Stamina maximums

### Modifying Enemy Behavior
Edit EnemyController script:
- Adjust stats in `ConfigureEnemyType()` method
- Change spawn rates in `GetRandomEnemyType()` method

### Adjusting Difficulty
Edit WaveSpawner component:
- `baseEnemiesPerWave`: Starting enemy count
- `difficultyScaling`: Multiplier per wave
- `timeBetweenWaves`: Rest period duration

### Arena Appearance
Edit ArenaBuilder component:
- Arena radius and wall height
- Pillar count and positions
- Torch count and placement
- Fog density and color

## Performance Optimization

- **Object Pooling**: Consider implementing for projectiles and enemies
- **LOD System**: Add Level of Detail for distant objects
- **Occlusion Culling**: Enable for large arenas
- **Light Baking**: Bake static lights for better performance
- **NavMesh Obstacles**: Add for dynamic cover

## Known Limitations

1. **Animations**: Basic implementation - requires animator controller setup
2. **Audio**: No sound effects or music implemented
3. **VFX**: Particle effects for attacks/impacts not included
4. **UI Polish**: Minimal UI styling
5. **Mobile Support**: Designed for PC with mouse/keyboard

## Future Enhancements

- [ ] Add animation controller with attack/dodge animations
- [ ] Implement sound effects and background music
- [ ] Create particle effects for combat
- [ ] Add power-ups and weapon variety
- [ ] Implement difficulty selection
- [ ] Add achievements and statistics
- [ ] Create multiple arena variations
- [ ] Add boss waves
- [ ] Implement online leaderboards

## Troubleshooting

### Player Won't Move
- Verify CharacterController is attached
- Check input axes in Project Settings
- Ensure PlayerController script has no errors

### Enemies Don't Spawn
- Bake NavMesh
- Verify WaveSpawner has enemy prefab assigned
- Check arena center reference is set

### Camera Issues
- Verify Camera is tagged "MainCamera"
- Check CameraController target is set to Player
- Ensure mouse input is not locked by other scripts

### UI Not Updating
- Verify UI elements are assigned in GameUIController
- Check Canvas is set to Screen Space - Overlay
- Ensure Player reference is found

## Credits

Game Design: Arena Survival Concept
Engine: Unity 2022.3.10f1
Scripts: Custom C# implementation
Architecture: Component-based design pattern

## License

See repository LICENSE file for details.
