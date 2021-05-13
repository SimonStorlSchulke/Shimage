using Godot;

public class CodeView : TextEdit
{
    void OnToggleCodeView(bool visibility) {
        this.Visible = visibility;
    }
}