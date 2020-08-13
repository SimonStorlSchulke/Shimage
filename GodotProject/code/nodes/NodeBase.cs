using Godot;
using System;

public abstract class NodeBase : GraphNode
{   
    public static int SLOTTYPE_RGB = 0, SLOTTYPE_RGBA = 1, SLOTTYPE_FLOAT = 2;
    public static Color SLOTCOLOR_RGB = new Color(1,.3f,.3f), SLOTCOLOR_RGBA = new Color(1,1,.3f), SLOTCOLOR_FLOAT = new Color(.6f,.6f,.6f), COLORBLACK = new Color(0,0,0);

    public string nodename;
    public override void _Ready() {
        Connect("close_request", this, nameof(onCloseRequest));
        Connect("resize_request", this, nameof(onResizeRequest));
    }

    void onCloseRequest() {
        QueueFree();
    }

    

    void onResizeRequest(Vector2 newSize) {
        RectSize = newSize;
    }

    public abstract void Execute();
}

public abstract class NodeFilter : NodeBase {
    //Maybe??
}