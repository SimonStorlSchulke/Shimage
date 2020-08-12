using Godot;
using System;

public class Viewer : Sprite
{
    public override void _Ready()
    {
        var Aspect = new Vector2(Texture.GetWidth(), Texture.GetHeight()).Normalized();
        Scale = new Vector2(1,1);
    }
}