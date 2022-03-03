using Godot;
using System;

public class WindowTitleBar : Control {

    bool dragging = false;
    Vector2 mouseStartPos;

    public override void _Ready() {
        base._Ready();
    }

    public void OnInput(InputEvent e) {
        if (e is InputEventMouseButton) {
            if ((e as InputEventMouseButton).ButtonIndex == 1) {
                dragging = !dragging;
                mouseStartPos = GetGlobalMousePosition();
            }
        }
    }

    public override void _Process(float delta) {
        if (dragging && !ResizeHandler.resizing) {
            OS.WindowPosition = OS.WindowPosition + GetGlobalMousePosition() - mouseStartPos;
        }
    }

    public void ToggleMaximize() {
        OS.WindowMaximized = !OS.WindowMaximized;
    }

    public void Minimize() {
        OS.WindowMinimized = true;
    }
}
