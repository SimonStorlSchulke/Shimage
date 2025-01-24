using Godot;

public partial class ViewArea : CenterContainer
{
    public override void _Ready()
    {
        GetViewport().Connect("FilesDropped", new Callable(this, nameof(OnDroppedFile)));
        
    }

    public void OnDroppedFile(string[] files, int screen) {
        //TODO - handle multiple files and layers
        GetNode<Shaderer>("Viewer").OnLoadImage(files[0]);
    }
}
