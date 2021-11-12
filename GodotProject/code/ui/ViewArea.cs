using Godot;
using System;

public class ViewArea : CenterContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetTree().Connect("files_dropped", this, nameof(OnDroppedFile));
    }

    public void OnDroppedFile(string[] files, int screen) {
        //TODO - handle multiple files and layers
        GetNode<Shaderer>("Viewer").OnLoadImage(files[0]);
    }
}
