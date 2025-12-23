# Deployment Guide

This guide covers deploying the Arena Survival Game to Render and Vercel platforms using automated WebGL builds via GitHub Actions.

## Table of Contents

- [Prerequisites](#prerequisites)
- [GitHub Secrets Setup](#github-secrets-setup)
- [Automated Build Process](#automated-build-process)
- [Deploying to Render](#deploying-to-render)
- [Deploying to Vercel](#deploying-to-vercel)
- [Manual Build Triggers](#manual-build-triggers)
- [WebGL Build Optimization](#webgl-build-optimization)
- [Troubleshooting](#troubleshooting)
- [Platform Comparison](#platform-comparison)

## Prerequisites

### Required Software
- **Unity Version**: 2022.3.10f1 (or compatible 2022.3.x LTS version)
- **Git**: For version control
- **GitHub Account**: With repository access
- **Unity License**: Personal, Plus, or Pro license

### Unity License Requirements
You need a valid Unity license to build via GitHub Actions. You can use:
- Unity Personal (free)
- Unity Plus
- Unity Pro

## GitHub Secrets Setup

Before the automated build can work, you must configure three GitHub secrets:

### 1. Obtain Unity License File

#### For Personal License (Free):
1. Install Unity on your local machine with the same version (2022.3.10f1)
2. Activate Unity with your credentials
3. Find the license file:
   - **Windows**: `C:\ProgramData\Unity\Unity_lic.ulf`
   - **macOS**: `/Library/Application Support/Unity/Unity_lic.ulf`
   - **Linux**: `~/.local/share/unity3d/Unity/Unity_lic.ulf`

#### Alternative Method (using Unity CLI):
```bash
# Request manual activation file
unity-editor -batchmode -createManualActivationFile

# This creates Unity_v[version].alf
# Upload this to: https://license.unity3d.com/manual
# Download the .ulf license file
```

### 2. Configure GitHub Secrets

Go to your GitHub repository:
1. Navigate to **Settings** → **Secrets and variables** → **Actions**
2. Click **New repository secret**
3. Add the following three secrets:

#### Secret 1: UNITY_LICENSE
- **Name**: `UNITY_LICENSE`
- **Value**: Contents of your `Unity_lic.ulf` file (copy entire file content)

#### Secret 2: UNITY_EMAIL
- **Name**: `UNITY_EMAIL`
- **Value**: Your Unity account email address

#### Secret 3: UNITY_PASSWORD
- **Name**: `UNITY_PASSWORD`
- **Value**: Your Unity account password

⚠️ **Security Note**: Never commit these secrets to your repository or share them publicly.

## Automated Build Process

### How It Works

1. **Trigger**: Push to `main` branch or manual workflow dispatch
2. **Build**: GitHub Actions runs Unity in headless mode to create WebGL build
3. **Cache**: Unity Library folder is cached for faster subsequent builds
4. **Output**: WebGL build is generated in `Build/WebGL/` directory
5. **Commit**: Build artifacts are committed to `webgl-build` branch
6. **Deploy**: Platforms pull from `webgl-build` branch

### Build Workflow

The workflow (`.github/workflows/unity-webgl-build.yml`) performs these steps:

1. Checks out the repository
2. Restores Unity Library cache (if available)
3. Activates Unity license
4. Builds WebGL target with optimizations
5. Commits build to `webgl-build` branch
6. Uploads artifacts for manual download

### Build Time
- **First build**: 15-30 minutes (no cache)
- **Subsequent builds**: 5-15 minutes (with cache)

## Deploying to Render

### Step 1: Connect Repository

1. Go to [Render Dashboard](https://dashboard.render.com/)
2. Click **New** → **Static Site**
3. Connect your GitHub repository
4. Grant Render access to the repository

### Step 2: Configure Static Site

Configure your static site with these settings:

- **Name**: `arena-survival-game` (or your preferred name)
- **Branch**: `webgl-build` ⚠️ Important: Use the build branch, not main
- **Build Command**: `echo "Using pre-built files"`
- **Publish Directory**: `Build/WebGL/WebGL`

### Step 3: Advanced Settings

Render will automatically read the `render.yaml` configuration which includes:

- WebAssembly MIME type headers
- Proper routing for single-page application
- Compression settings
- Cache control headers

### Step 4: Deploy

1. Click **Create Static Site**
2. Render will deploy from the `webgl-build` branch
3. Wait for deployment to complete (2-5 minutes)
4. Access your game at the provided URL

### Step 5: Automatic Updates

Every time you push to `main`:
1. GitHub Actions rebuilds WebGL
2. Updates `webgl-build` branch
3. Render automatically redeploys

## Deploying to Vercel

### Step 1: Install Vercel CLI (Optional)

```bash
npm install -g vercel
```

Or use the Vercel Dashboard (recommended for first deployment).

### Step 2: Connect Repository via Dashboard

1. Go to [Vercel Dashboard](https://vercel.com/dashboard)
2. Click **Add New** → **Project**
3. Import your GitHub repository
4. Grant Vercel access

### Step 3: Configure Project

Configure the project with these settings:

- **Framework Preset**: Other
- **Root Directory**: `./` (leave as root)
- **Build Command**: Leave empty or use: `echo "Using pre-built files"`
- **Output Directory**: `Build/WebGL/WebGL`
- **Install Command**: Leave empty

### Step 4: Configure Branch

⚠️ **Important**: Configure Vercel to deploy from the `webgl-build` branch:

1. After importing, go to **Settings** → **Git**
2. Under **Production Branch**, change from `main` to `webgl-build`
3. Save changes

### Step 5: Deploy

1. Click **Deploy**
2. Vercel will read `vercel.json` configuration automatically
3. Wait for deployment (1-3 minutes)
4. Access your game at the provided URL

### Step 6: Custom Domain (Optional)

1. Go to **Settings** → **Domains**
2. Add your custom domain
3. Follow DNS configuration instructions

### Vercel CLI Deployment

If using CLI:

```bash
# First time setup
vercel login
vercel link

# Deploy to production
vercel --prod

# Deploy from specific branch
git checkout webgl-build
vercel --prod
```

## Manual Build Triggers

### Via GitHub Actions UI

1. Go to your repository on GitHub
2. Click **Actions** tab
3. Select **Unity WebGL Build** workflow
4. Click **Run workflow** button
5. Select branch (usually `main`)
6. Click **Run workflow**

This is useful for:
- Rebuilding without code changes
- Testing the build pipeline
- Creating a fresh build after fixing build issues

### Via GitHub CLI

```bash
# Install GitHub CLI
# https://cli.github.com/

# Trigger workflow
gh workflow run "Unity WebGL Build"

# Check workflow status
gh run list --workflow="Unity WebGL Build"
```

## WebGL Build Optimization

### Build Size Reduction

The WebGL build can be large (50-200+ MB). Here are optimization tips:

#### 1. Enable Compression
In Unity Editor:
- **File** → **Build Settings** → **Player Settings**
- **Publishing Settings** → **Compression Format**: Choose `Brotli` (best) or `Gzip`

#### 2. Code Stripping
- **Player Settings** → **Other Settings**
- **Managed Stripping Level**: `Medium` or `High`
- **Strip Engine Code**: Enable

#### 3. Reduce Asset Quality

For textures:
- Select texture in Project window
- Inspector → **Max Size**: Reduce to 1024 or 512
- **Compression**: Use appropriate format

For audio:
- Inspector → **Compression Format**: `Vorbis`
- **Quality**: 70-100

#### 4. Optimize Scene

- Reduce enemy count in WaveSpawner
- Lower torch count in ArenaBuilder (from 12 to 6-8)
- Reduce fog density
- Use simpler materials

#### 5. Unity WebGL Build Settings

In your project, recommended settings:
```
Publishing Settings:
- Compression Format: Brotli
- Include Debug Symbols: Disabled
- Data caching: Enabled

Resolution and Presentation:
- Default Canvas Width: 1280 (instead of 1920)
- Default Canvas Height: 720 (instead of 1080)
- Run In Background: Enabled

Other Settings:
- Scripting Backend: IL2CPP
- API Compatibility Level: .NET Standard 2.1
- Managed Stripping Level: Medium
```

### Performance Optimization

For better WebGL performance:

1. **Reduce Draw Calls**
   - Enable Static Batching
   - Use GPU Instancing on materials
   - Combine meshes where possible

2. **Optimize Lighting**
   - Reduce real-time lights (use baked lighting)
   - Reduce shadow quality
   - Lower shadow distance

3. **Simplify Post-Processing**
   - Disable expensive effects
   - Reduce anti-aliasing quality

4. **Memory Management**
   - Player Settings → **WebGL Memory Size**: Start with 256MB
   - Increase only if needed

## Troubleshooting

### Build Fails in GitHub Actions

#### Issue: "License activation failed"
**Solution:**
- Verify `UNITY_LICENSE` secret contains complete .ulf file content
- Check `UNITY_EMAIL` and `UNITY_PASSWORD` are correct
- Ensure Unity account has an active license
- Try regenerating the license file

#### Issue: "Build failed with errors"
**Solution:**
- Check the Actions log for specific errors
- Ensure the project builds successfully in Unity Editor first
- Verify all scenes are properly set up
- Check for missing scripts or assets

#### Issue: "Out of memory during build"
**Solution:**
- Unity WebGL builds require significant memory
- This usually resolves itself on retry
- GitHub Actions runners have 7GB RAM

### Deployment Issues

#### Issue: Game doesn't load (blank screen)
**Solution:**
- Check browser console for errors (F12)
- Verify MIME types are correct (should see `application/wasm` for .wasm files)
- Check `Cross-Origin` headers are set correctly
- Ensure all build files are present in deployment

#### Issue: "Incorrect MIME type" error
**Solution:**
- For Render: Verify `render.yaml` headers configuration
- For Vercel: Verify `vercel.json` routes configuration
- Check web server is serving .wasm files with correct Content-Type

#### Issue: Loading takes too long
**Solution:**
- Enable compression (Brotli/Gzip) in Unity build settings
- Optimize assets (see optimization section)
- Use CDN features of hosting platform
- Consider using Unity's AssetBundle system

#### Issue: Game crashes or freezes
**Solution:**
- Check browser console for memory errors
- Increase WebGL Memory Size in Player Settings
- Reduce graphics quality
- Test in different browsers (Chrome/Firefox recommended)

### Platform-Specific Issues

#### Render Issues

**Issue: "Build command failed"**
- Render should use pre-built files, ensure branch is `webgl-build`
- Build command should be: `echo "Using pre-built files"`

**Issue: "Deploy failed"**
- Check that `Build/WebGL/WebGL` directory exists in webgl-build branch
- Verify render.yaml is present in webgl-build branch

#### Vercel Issues

**Issue: "Build failed"**
- Ensure Production Branch is set to `webgl-build`
- Leave Install Command empty (no need to install dependencies)
- Verify Output Directory is `Build/WebGL/WebGL`

**Issue: "404 on page refresh"**
- Check rewrites configuration in vercel.json
- Ensure SPA routing is configured correctly

### Testing Deployments

Test checklist after deployment:
- [ ] Game loads without errors
- [ ] Player controls work (WASD, mouse, space, clicks)
- [ ] Enemies spawn correctly
- [ ] UI elements display properly
- [ ] Game over and restart work
- [ ] Performance is acceptable (30+ FPS)
- [ ] Works in Chrome, Firefox, and Safari
- [ ] Mobile browser compatibility (if applicable)

## Platform Comparison

### Render
**Pros:**
- Simple configuration with render.yaml
- Good for static sites
- Automatic SSL
- GitHub integration

**Cons:**
- Slower cold starts
- Less CDN optimization than Vercel
- Free tier has limitations

**Best for:** Simple deployments, testing, cost-conscious projects

### Vercel
**Pros:**
- Excellent CDN performance
- Fast deployments (1-3 minutes)
- Superior edge network
- Great developer experience
- Automatic preview deployments

**Cons:**
- Bandwidth limits on free tier
- Commercial projects may need Pro plan

**Best for:** Production deployments, high-traffic games, optimal performance

### Recommendation

- **Development/Testing**: Use Render (simpler setup)
- **Production**: Use Vercel (better performance)
- **Both**: You can deploy to both simultaneously!

## Additional Resources

### Unity WebGL Documentation
- [Unity WebGL Player Settings](https://docs.unity3d.com/Manual/webgl-building.html)
- [WebGL Browser Compatibility](https://docs.unity3d.com/Manual/webgl-browsercompatibility.html)
- [WebGL Performance Optimization](https://docs.unity3d.com/Manual/webgl-performance.html)

### Platform Documentation
- [Render Static Sites](https://render.com/docs/static-sites)
- [Vercel Configuration](https://vercel.com/docs/project-configuration)
- [GitHub Actions for Unity](https://game.ci/docs)

### Community & Support
- [Unity Forums](https://forum.unity.com/)
- [Game CI Discord](https://game.ci/discord)
- Unity Hub → Community

## Updates and Maintenance

### Keeping Dependencies Updated

Periodically update:
- Unity version (stay on LTS)
- GitHub Actions versions in workflow
- Platform configurations as needed

### Monitoring Deployments

Both platforms provide:
- Deployment logs
- Analytics
- Performance monitoring
- Error tracking

Check these regularly to ensure smooth operation.

---

## Quick Reference

### GitHub Secrets Required
```
UNITY_LICENSE - Unity license file (.ulf) content
UNITY_EMAIL - Unity account email
UNITY_PASSWORD - Unity account password
```

### Deployment Branches
```
main - Source code
webgl-build - Compiled WebGL build (auto-generated)
```

### Build Times
```
First build: 15-30 minutes
Cached builds: 5-15 minutes
Deployment: 1-5 minutes
```

### Support
For issues or questions:
1. Check GitHub Actions logs
2. Review platform deployment logs
3. Check browser console (F12)
4. Consult this documentation
5. Open an issue in the repository

---

**Happy Deploying! 🚀🎮**
