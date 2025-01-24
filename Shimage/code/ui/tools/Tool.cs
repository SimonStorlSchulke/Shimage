using Godot;
using System;

public abstract partial class Tool : Control
{
    public Node connectedTo;
    public Node controlledBy;

    public override void _Ready() {
        LayerManager.instance.Connect("SLayerSelected", new Callable(this, nameof(OnLayerSelected)));
    }

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

        foreach (var signal in GetSignalList()) {
            var connections = GetSignalConnectionList(nameof(signal));
            foreach (var connection in connections) {
                GD.Print(connection.GetType());
            }
        }

        connectedTo = null;
        foreach (Tool tool in ToolsLayer.instance.GetChildren()) {
            tool.Visible = false;
            tool.connectedTo = null;
            tool.SetProcess(false);
        }
    }

    public virtual void OnLayerSelected() {
    }
}
