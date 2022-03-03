using Godot;
using System;

public class ResizeHandler : Control {
    [Export]
    int borderWidth = 5;

    public static bool resizing;
    Vector2 mouseStartPos;
    int mArea;

    public override void _Ready() {

    }

    /// <summary>Returns 1-8 if Cursor is in Topleft Corner, Top, Topright Corner, Right, Bottomright Corner... and 0 if not in any.</summary>
    int GetMouseInArea() {
        Vector2 mPos = GetLocalMousePosition() * 1.15f; // apparently has to be multiplied by window shrink factor... sad.
        Vector2 wSize = OS.WindowSize;
        bool left = mPos.y < borderWidth;
        bool top = mPos.y < borderWidth;
        bool right = mPos.x > (wSize.x - borderWidth);
        bool bottom = mPos.y > (wSize.y - borderWidth);
        GD.Print(mPos.x, "   ", wSize.x - borderWidth);


        if (left && top)
            return 1;
        if (right && top)
            return 3;
        if (right && bottom)
            return 5;
        if (left && bottom)
            return 7;
        if (top)
            return 2;
        if (right)
            return 4;
        if (bottom)
            return 6;
        if (left)
            return 8;
        return 0;
    }

    void Resize(int mArea) {
        if (mArea == 1) {
            OS.WindowSize = new Vector2(OS.WindowSize.x, OS.WindowSize.y + GetGlobalMousePosition().y - mouseStartPos.y);
        }
    }

    /*
    public override void _Input(InputEvent e)
    {
        GD.Print(GetMouseInArea());
        if (e is InputEventMouseButton) {
            mArea = GetMouseInArea();
            if ((e as InputEventMouseButton).ButtonIndex == 1 && mArea != 0) {
                mouseStartPos = GetGlobalMousePosition();
                resizing = !resizing;
            }
        }
    }

    public override void _Process(float delta)
    {
        if (resizing) Resize(mArea);
    }
    */
}
