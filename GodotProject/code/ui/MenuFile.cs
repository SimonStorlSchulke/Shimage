using Godot;

public partial class MenuFile : Button
{

    FileDialog fileDialog;

    Image img = new Image();
    ImageTexture tex = new ImageTexture();

    [Signal]
    public delegate void SLoadImageEventHandler(string path);
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fileDialog = GetNode<FileDialog>("FileDialog");
        
        fileDialog.Filters = new string[]{
            "*.bmp ; BMP",
            "*.jpg ; JPG",
            "*.jpeg ; JPEG",
            "*.png ; PNG",
            "*.svg ; SVG",
            "*.tga ; TGA",
            "*.webp ; WEBP",
            };
    }

    public void OnOpenFile() {
        fileDialog.CurrentDir = AppHandler.imagePath.GetBaseDir();
        fileDialog.PopupCentered();
    }
}
