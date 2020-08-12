using Godot;
using System;

public class ImageSource : NodeBase {
    TextEdit pathInput;
    TextureRect viewer;

    Image img = new Image();
    ImageTexture tex = new ImageTexture();

    public override void _Ready() {
        base._Ready();
        pathInput = GetNode<TextEdit>("VBoxContainer/HBoxContainer/Path");
        viewer = GetNode<TextureRect>("VBoxContainer/ImageViewer");
    }

    public void OnPathUpdate() {
        //Draw Image -> extract Method later
        img.Load(pathInput.Text);
        if (img == null)
            return;
        tex.CreateFromImage(img);
        viewer.Texture = tex;
    }
}
