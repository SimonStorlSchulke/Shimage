using Godot;

public class MenuFile : MenuButton
{

    FileDialog fileDialog;
    FileDialog fileDialogSave;

    [Export]
    NodePath PathViewer = new NodePath();

    Image img = new Image();
    ImageTexture tex = new ImageTexture();

    [Signal]
    public delegate void SLoadImage(string path);

    [Signal]
    public delegate void SSaveImage(string path);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fileDialog = GetNode<FileDialog>("FileDialog");
        fileDialogSave = GetNode<FileDialog>("FileDialogSave");
        //viewer = GetNode<Viewer>(PathViewer);

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

        //this.Connect(nameof(SLoadImage), viewer, nameof(viewer.OnLoadImage));
        //this.Connect(nameof(SSaveImage), viewer, nameof(viewer.OnSaveImage));
    }

    public void OnItemPressed(int id) {
        if (GetPopup().GetItemText(id) == "Open Image") {
            fileDialog.PopupCentered();
        }
        if (GetPopup().GetItemText(id) == "Save Image") {
            fileDialogSave.PopupCentered();
        }
    }

    public void OnFileSelected(string path) {
        this.EmitSignal(nameof(SLoadImage), path);
    }

    public void OnSaveSelected(string path) {
        this.EmitSignal(nameof(SSaveImage), path);
    }

}
