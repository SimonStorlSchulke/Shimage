using Godot;
using System;

public class MenuFile : MenuButton
{

    FileDialog fileDialog;

    [Export]
    NodePath PathViewer = new NodePath();
    
    Viewer viewer = new Viewer();
    Image img = new Image();
    ImageTexture tex = new ImageTexture();

    [Signal]
    public delegate void SLoadImage(string path);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fileDialog = GetNode<FileDialog>("FileDialog");
        viewer = GetNode<Viewer>(PathViewer);

        // TODO -only jpg allowed for now until transparency is supported
        fileDialog.Filters = new string[]{
            //"*.bmp ; BMP",
            "*.jpg ; JPG",
            "*.jpeg ; JPEG",
            //"*.png ; PNG",
            //"*.svg ; SVG",
            //"*.tga ; TGA",
            //"*.webp ; WEBP",
            };

        this.GetPopup().AddItem("Open Image");
        this.GetPopup().AddItem("Save Image");
        this.GetPopup().Connect("id_pressed", this, nameof(OnItemPressed));

        this.Connect(nameof(SLoadImage), viewer, nameof(viewer.OnLoadImage));
    }

    public void OnItemPressed(int id) {
        if (GetPopup().GetItemText(id) == "Open Image") {
            fileDialog.PopupCentered();
        }
    }

    public void OnFileSelected(string path) {
        this.EmitSignal(nameof(SLoadImage), path);
    }

}
