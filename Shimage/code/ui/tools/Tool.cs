using Godot;
using System;

public abstract class Tool : Control
{
    public Node connectedTo;
    public virtual void ActivateTool(Node connectedTo) {
        GD.Print("Activate Move Tool");
        foreach (Control tool in GetParent().GetChildren()) {
            tool.Visible = false;
        }
        this.connectedTo = connectedTo;
        this.Visible = true;
    }
}
