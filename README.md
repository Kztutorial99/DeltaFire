# DeltaFire

Android-first 3D battle royale FPS prototype built with Unity.

## Prototype goals

- First-person controller
- Mobile touch controls
- Shooting, reload and damage
- Loot/inventory foundation
- Bots
- Shrinking safe zone
- Last-player-standing loop
- Android build pipeline

## Unity

Target: Unity 6 LTS (6000.x).

## Project setup

1. Open the repository folder in Unity Hub.
2. Open **Tools > DeltaFire > Create Prototype Scene**.
3. Open `Assets/Scenes/DeltaFirePrototype.unity`.
4. Press Play for the local prototype.
5. For Android, switch the build target to Android and configure the required Unity Android modules.

The GitHub Actions workflow is a build template. A Unity license/activation method must be configured in repository secrets before a cloud build can run.

## Architecture

`Assets/Scripts/Core` - game state and match loop  
`Assets/Scripts/Player` - FPS player and weapon systems  
`Assets/Scripts/AI` - bot logic  
`Assets/Scripts/World` - safe zone and loot foundations  
`Assets/Scripts/UI` - mobile HUD foundation  
`Assets/Editor` - prototype scene generator
