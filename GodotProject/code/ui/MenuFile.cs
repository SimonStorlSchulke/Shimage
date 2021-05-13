using Godot;

public class MenuFile : Button
{

    FileDialog fileDialog;

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
        fileDialog.PopupCentered();
    }
}
