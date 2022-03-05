using Godot;
using System;

public class ToolsLayer : Control {
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
            instance.RectScale = Apphandler.currentViewer.RectScale;
            instance.RectGlobalPosition = Apphandler.currentViewer.RectGlobalPosition;
            Control handle = activeTool.GetChild<Control>(0);
            handle.RectScale = Vector2.One / Apphandler.currentViewer.RectScale;
    }

    public override void _Input(InputEvent e) {
        if (e is InputEventMouseButton me) {
            if (me.ButtonIndex == (int)ButtonList.Right && me.Pressed) {
                DeactivateTool();
            }
        }
    }

    public static void DeactivateTool() {
        GD.Print("Deactivate Tool");
        foreach (Tool tool in instance.GetChildren()) {
            tool.Visible = false;
            tool.connectedTo = null;
            tool.SetProcess(false);
        }
    }

}
