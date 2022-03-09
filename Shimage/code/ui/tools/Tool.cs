using Godot;
using System;

public abstract class Tool : Control
{
    public Node connectedTo;
    public Node controlledBy;

    public virtual void ActivateTool(Node connectedTo, Godot.Collections.Array args = null) {
        foreach (Tool tool in ToolsLayer.instance.GetChildren()) {
            tool.DeactivateTool();
        }
        this.connectedTo = connectedTo;
        this.Visible = true;
        ToolsLayer.activeTool = this;
    }

    /// <summary> Don't forget to first disconnect existing Signals in child overrides of this method </summary>
    public virtual void DeactivateTool() {
        foreach (Tool tool in ToolsLayer.instance.GetChildren()) {
            tool.Visible = false;
            tool.connectedTo = null;
            tool.SetProcess(false);
        }
    }
}
