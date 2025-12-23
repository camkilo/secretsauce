# Build Instructions

## Building the Game

### Prerequisites
- Unity 2022.3.10f1 or later
- Completed scene setup (see QUICKSTART.md)

### Build for Windows

1. **Open Build Settings**
   - File → Build Settings
   - Select "PC, Mac & Linux Standalone"
   - Target Platform: Windows
   - Architecture: x86_64

2. **Configure Player Settings**
   - Click "Player Settings"
   - Company Name: Your name/studio
   - Product Name: Arena Survival
   - Default Icon: (optional)

3. **Add Scene**
   - Click "Add Open Scenes" to add MainArena.unity
   - Or drag Assets/Scenes/MainArena.unity into "Scenes In Build"

4. **Build**
   - Click "Build" or "Build And Run"
   - Choose output folder
   - Wait for build to complete
   - Executable will be in chosen folder

### Build for macOS

1. **Open Build Settings**
   - File → Build Settings
   - Select "PC, Mac & Linux Standalone"
   - Target Platform: macOS

2. **Configure**
   - Architecture: Intel 64-bit or Apple Silicon
   - Follow steps 2-4 from Windows instructions

### Build for Linux

1. **Open Build Settings**
   - File → Build Settings
   - Select "PC, Mac & Linux Standalone"
   - Target Platform: Linux

2. **Configure**
   - Architecture: x86_64
   - Follow steps 2-4 from Windows instructions

### Build for WebGL

1. **Open Build Settings**
   - File → Build Settings
   - Select "WebGL"

2. **Configure WebGL Settings**
   - Player Settings → Resolution and Presentation
   - Default Canvas Width: 1920
   - Default Canvas Height: 1080

3. **Optimization**
   - Player Settings → Publishing Settings
   - Enable "Compression Format: Gzip"
   - Disable "Development Build" for production

4. **Build**
   - Click "Build"
   - Choose output folder
   - Host the generated files on a web server

**Note**: WebGL may have performance limitations. Consider:
- Reducing max enemies per wave
- Lowering graphics quality
- Reducing torch count

## Build Optimization

### Release Build Settings

1. **Player Settings → Other Settings**
   - Scripting Backend: IL2CPP (better performance)
   - API Compatibility Level: .NET Standard 2.1
   - Managed Stripping Level: Medium or High

2. **Quality Settings**
   - Edit → Project Settings → Quality
   - For release builds, use "High" or "Medium" preset
   - Adjust Shadow Distance for performance

3. **Graphics Settings**
   - Disable unnecessary graphics APIs
   - Enable Static Batching
   - Enable GPU Instancing on materials

### Size Optimization

1. **Texture Compression**
   - Select textures in Project window
   - Inspector → Compress with appropriate format
   - DXT5 for Windows/Linux
   - ASTC for mobile

2. **Audio Compression**
   - Select audio files
   - Inspector → Compression Format: Vorbis
   - Quality: 70-100

3. **Code Stripping**
   - Player Settings → Other Settings
   - Managed Stripping Level: Medium or High
   - Strip Engine Code: Enabled

## Distribution

### Windows
- Zip the entire build folder
- Include any required Visual C++ redistributables
- Provide README with controls

### macOS
- The .app bundle is the application
- Right-click → Compress to create .zip
- Consider code signing for distribution

### Linux
- Tar.gz the build folder
- Ensure execute permissions on binary
- Test on target distribution

### WebGL
- Upload build folder to web hosting
- Ensure correct MIME types are served
- Test in different browsers

## Testing Builds

### Before Release Checklist

- [ ] All scenes load correctly
- [ ] Player controls work properly
- [ ] Enemies spawn and behave correctly
- [ ] UI displays all information
- [ ] Game over/restart works
- [ ] No console errors
- [ ] Acceptable performance (30+ FPS)
- [ ] Audio works (if implemented)
- [ ] Resolution options work
- [ ] Fullscreen toggle works

### Performance Testing

Monitor these metrics:
- **FPS**: Should maintain 60 FPS on target hardware
- **Memory**: Keep under 2GB for 32-bit builds
- **Load Time**: Scene should load in under 5 seconds
- **Build Size**: Try to keep under 500MB

## Common Build Issues

### Issue: Scripts don't compile
**Solution**: 
- Check for compilation errors in Console
- Ensure all namespaces are correct
- Verify Unity version compatibility

### Issue: Scenes are missing
**Solution**:
- Add all scenes to Build Settings
- Verify scene paths are correct

### Issue: NavMesh doesn't work in build
**Solution**:
- Bake NavMesh before building
- Ensure Navigation package is installed
- Check that NavMesh data is included in build

### Issue: Low performance in build
**Solution**:
- Reduce enemy count
- Lower graphics quality
- Optimize lighting (less torches, no real-time shadows)
- Enable static batching
- Use IL2CPP scripting backend

### Issue: UI doesn't display
**Solution**:
- Check Canvas Scaler settings
- Verify UI elements are assigned in Inspector
- Ensure EventSystem exists in scene

## Platform-Specific Notes

### Windows
- Requires DirectX 11 or later
- Consider both 64-bit and 32-bit builds
- Test on Windows 10 and 11

### macOS
- May require Gatekeeper bypass for unsigned builds
- Test on both Intel and Apple Silicon if possible
- Consider minimum macOS version support

### Linux
- Test on Ubuntu/Debian based systems
- Provide dependencies list
- Include launcher script if needed

### WebGL
- Requires WebGL 2.0 support in browser
- Best performance in Chrome/Edge
- Consider mobile browser limitations
- Requires web server (won't work with file://)

## Publishing

When ready to publish:

1. **Create marketing materials**
   - Screenshots
   - Gameplay video
   - Feature list
   - System requirements

2. **Prepare distribution**
   - Create installer (optional)
   - Write installation instructions
   - Include license information

3. **Choose platform**
   - Steam
   - Itch.io
   - Game Jolt
   - Your own website

4. **Update version numbers**
   - Project Settings → Player → Version

## Support

For build-related issues:
- Check Unity Console for errors
- Review Player.log file in build folder
- Test in Unity Editor first
- Verify all assets are included

Happy building! 🎮🚀
