using Godot;
using System;

public partial class ToolsLayer : Control {
    public static ToolsLayer instance;

    public static Tool activeTool;

    public override void _EnterTree() {
        instance = this;
    }
    public override void _Ready() {
        foreach (Control tool in GetChildren()) {
            tool.Visible = false;;
        }
    }

    public static void UpdateTransform() {
        if (activeTool != null)
            instance.Scale = Apphandler.currentViewer.Scale;
            instance.GlobalPosition = Apphandler.currentViewer.GlobalPosition;
            Control handle = activeTool.GetChild<Control>(0);
            handle.Scale = Vector2.One / Apphandler.currentViewer.Scale;
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouseButton me) {
            if (me.ButtonIndex == MouseButton.Right && me.Pressed) {
                activeTool.DeactivateTool();
            }
        }
    }
}
