using Godot;
using System;

public partial class WindowTitleBar : Control {

    bool dragging = false;
    Vector2 mouseStartPos;

    public override void _Ready() {
        base._Ready();
    }

    public void OnInput(InputEvent e) {
        if (e is InputEventMouseButton) {
            if ((e as InputEventMouseButton).ButtonIndex == MouseButton.Left) {
                dragging = !dragging;
                mouseStartPos = GetGlobalMousePosition();
            }
        }
    }

    public override void _Process(double delta) {
        if (dragging && !ResizeHandler.resizing) {
            DisplayServer.WindowSetPosition(DisplayServer.WindowGetPosition() + (Vector2I)GetGlobalMousePosition() - (Vector2I)mouseStartPos);
        }
    }

    public void ToggleMaximize() {
        //OS.WindowMaximized = !OS.WindowMaximized;
    }

    public void Minimize() {
       // OS.WindowMinimized = true;
    }
}
