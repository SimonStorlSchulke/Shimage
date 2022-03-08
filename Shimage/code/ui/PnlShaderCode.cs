using Godot;
using System;

public class PnlShaderCode : Panel
{
    [Export] NodePath NPBtnShow;
    
    bool expanded;

    Vector2 sizeExpanded = new Vector2(600,0);
    Vector2 sizeCollapsed = new Vector2(30,0);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void ToggleVisibility() {
        RectMinSize = expanded ? sizeCollapsed : sizeExpanded;
        expanded = !expanded;
    }
}