# Shimage

A Photo Editor with a Filter Stack. Based on Shaders to work in Realtime. Written in C# and GLSL with Godot. See [Releases.](https://github.com/SimonStorlSchulke/GodotPhotoEdit/releases)
The *Layers-Rewrite* Branch is WIP and supports multiple Projects, Layers with Realtime Filters, Blendmodes and basic Transform Tools.

Saving Images is based on [Coldragons ShaderToImage](https://github.com/Coldragon/godot-shader-to-image)


![2022-07-01 10_31_15-main tscn - Shimage - Godot Engine](https://user-images.githubusercontent.com/25198913/176857068-037c72f3-8c77-4dcb-b74b-62014d92a715.jpg)
*Layers-Rewrite* Branch (wip)

![img](img.png)
*master* Branch

## Ideas
- More Filters 
  - RGB Curves(UI ?))
  - Gausian Blur? Tilt Shift...
    - allows Bloom (Blur with lum Threshold)
- Masks
  - Mask drawing
- Overlay Images (Blendmode, Scale, Rot, Pos, Mask)
  - Blendmodes: Normal, Multiply, Add, Subtract, Divide, Overlay, Screen
- Procedural Textures
- Presets
- Batch Processing
