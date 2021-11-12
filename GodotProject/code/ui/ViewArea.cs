using Godot;

public class ViewArea : CenterContainer
{
    public override void _Ready()
    {
        GetTree().Connect("files_dropped", this, nameof(OnDroppedFile));
    }

    public void OnDroppedFile(string[] files, int screen) {
        //TODO - handle multiple files and layers
        GetNode<Shaderer>("Viewer").OnLoadImage(files[0]);
    }
}
