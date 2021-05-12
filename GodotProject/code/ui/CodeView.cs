using Godot;
using System;

public class CodeView : TextEdit
{
    void OnToggleCodeView(bool visibility) {
        this.Visible = visibility;
    }
}