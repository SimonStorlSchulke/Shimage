using Godot;
using System;

public abstract class NodeBase : GraphNode
{

    public override void _Ready() {
        Connect("close_request", this, nameof(onCloseRequest));
        Connect("resize_request", this, nameof(onResizeRequest));
    }

    void onCloseRequest() {
        GD.Print("DELETE");
        QueueFree();

    }

    void onResizeRequest(Vector2 newSize) {
        RectSize = newSize;
    }
}
