using Godot;
using System;

public class ImageSource : NodeBase {
    TextEdit pathInput;
    TextureRect viewer;

    Image img = new Image();
    ImageTexture tex = new ImageTexture();
    FileDialog fileDialog;

    public override void _Ready() {
        base._Ready();
        fileDialog = GetNode<FileDialog>("FileDialog");
        pathInput = GetNode<TextEdit>("VBoxContainer/HBoxContainer/Path");
        viewer = GetNode<TextureRect>("VBoxContainer/ImageViewer");

        fileDialog.Filters = new string[]{
            "*.bmp ; BMP",
            "*.jpg ; JPG",
            "*.jpeg ; JPEG",
            "*.png ; PNG",
            "*.svg ; SVG",
            "*.tga ; TGA",
            "*.webp ; WEBP",
            };
        fileDialog.PopupCentered();
    }

    void setImage(string path) {
        //Draw Image -> extract Method later
        img.Load(path);
        if (img == null)
            return;
        tex.CreateFromImage(img);
        viewer.Texture = tex;
    }

    void OnOpenFileDialog() {
       fileDialog.PopupCentered();
    }

    void OnPathUpdate() {
        setImage(pathInput.Text);
    }

    void OnFileSelected(string path) {
        pathInput.Text = path;
        setImage(path);
    }


}
