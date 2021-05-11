using Godot;
using System;

public class ShaderToImage : Node2D
{

    Image generatedImage;

    Sprite drawer;
    TextureRect shaderContainer;
    Viewport viewport;

    public override void _Ready() {
        drawer =  GetNode<Sprite>("Sprite");
        shaderContainer =  GetNode<TextureRect>("Viewport/TextureRect");
        viewport = GetNode<Viewport>("Viewport");
    }

    public Image GetImage() {
        if (generatedImage != null) {
            return generatedImage;
        } else {
            GD.Print("No image generated, use generate_image() and wait for \"generated\" signal");
            return null;
        }
    }

    public void GenerateImage(Material material, Vector2 res, float multiplier) {
        generatedImage = (Image)drawer.Texture.GetData().Duplicate();
    }
}