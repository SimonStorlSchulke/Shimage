using Godot;
using System;

public partial class PnlShaderCode : Panel
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    bool collapsed = false;
    public void ToggleCollapsed() {
        collapsed = !collapsed;
        OffsetLeft = collapsed ? -32 : -632;
    }

}
