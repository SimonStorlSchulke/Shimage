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
            activeTool.GetChild<Control>(0).RectScale = Vector2.One / Apphandler.currentViewer.RectScale;
    }

}
