using Godot;
using System;

public partial class ShaderToImage : Node2D
{

    Image generatedImage;

    Sprite2D drawer;
    TextureRect shaderContainer;
    SubViewport viewport;

    public override void _Ready() {
        drawer =  GetNode<Sprite2D>("Sprite2D");
        shaderContainer =  GetNode<TextureRect>("SubViewport/TextureRect");
        viewport = GetNode<SubViewport>("SubViewport");
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
        generatedImage = (Image)drawer.Texture.Duplicate();
    }
}