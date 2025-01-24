using Godot;

public partial class CodeView : TextEdit
{
    void OnToggleCodeView(bool visibility) {
        this.Visible = visibility;
    }
}