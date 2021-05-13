using Godot;
using System;

public class CodeView : TextEdit
{
    void OnToggleCodeView(bool visibility) {
        this.Visible = visibility;
        this.RectMinSize = new Vector2(this.RectSize.x, 150);
    }
}