# Quick Start Guide - Arena Survival Game

## Fastest Way to Get Started

### Method 1: Using Auto-Setup Script (Recommended)

1. **Open Unity Project**
   - Clone the repository or download the source code
   - Open Unity Hub
   - Add project from the repository folder
   - Open with Unity 2022.3.10f1+

2. **Create New Scene**
   - File → New Scene
   - Choose "3D (Built-in Render Pipeline)"
   - File → Save As → `Assets/Scenes/MainArena.unity`

3. **Auto Setup**
   - Create an empty GameObject (right-click in Hierarchy → Create Empty)
   - Name it "SceneSetup"
   - Add Component → GameSceneSetup script
   - The script will automatically create:
     - Player with CharacterController
     - Arena with walls, floor, and pillars
     - Game Manager
     - Wave Spawner
     - Camera Controller
     - UI Canvas

4. **Bake Navigation**
   - Select all arena floor objects
   - In Inspector, check "Navigation Static"
   - Window → AI → Navigation
   - Click "Bake" button

5. **Play the Game**
   - Press Play button in Unity Editor
   - Start fighting waves of enemies!

### Method 2: Manual Setup

If you prefer manual setup, follow the detailed instructions in `DOCUMENTATION.md`.

## Essential Controls

- **Move**: WASD
- **Camera**: Mouse movement
- **Dodge**: Space bar
- **Light Attack**: Left Mouse Button
- **Heavy Attack**: Right Mouse Button
- **Restart**: R (after death)

## Quick Testing

After setup, you should immediately see:
- ✓ Player capsule at center of arena
- ✓ Circular stone arena with walls
- ✓ 8 stone pillars around the arena
- ✓ 12 flickering torch lights
- ✓ Fog atmosphere
- ✓ UI showing health, stamina, timer, and wave count

## First Wave Starts

- Wait 3 seconds after pressing Play
- First wave spawns 3 enemies
- Enemies will navigate toward you
- Fight, dodge, survive!

## Troubleshooting

**Player doesn't move?**
- Check that CharacterController is attached
- Verify Input axes exist (Edit → Project Settings → Input Manager)

**Enemies don't move?**
- Make sure you baked the NavMesh (see step 4 above)
- Check that enemies have NavMeshAgent component

**Can't see anything?**
- Ensure Main Camera exists and is tagged "MainCamera"
- Check that Directional Light is in scene

**No UI?**
- GameUIController needs UI elements assigned
- You can play without UI, stats still work in background

## Next Steps

1. **Add Animations**: Create an Animator Controller with attack/dodge animations
2. **Improve Visuals**: Replace primitive shapes with 3D models
3. **Add Audio**: Import and assign sound effects and music
4. **Customize**: Tweak values in script components to adjust gameplay

## Performance Tips

- Lower enemy count per wave if performance is slow
- Reduce torch count for better FPS
- Disable shadows on some lights
- Use simpler materials

## Need Help?

- Check `DOCUMENTATION.md` for detailed information
- Review script comments for technical details
- Inspect example scene values in Editor
- Debug with `DebugVisualizer` component

**Have fun surviving!** 🎮⚔️
