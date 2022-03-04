using Godot;

public class Mover : Tool {
    [Signal] delegate void Moved(Vector2 to);
    [Export]
    Vector2 handleSize = new Vector2(32, 32);
    ColorRect handle;
    bool moving;
    Vector2 mouseStartPos = new Vector2();
    //Vector2 handleStartPos = new Vector2();


    public override void ActivateTool(Node connectedTo) {
        base.ActivateTool(connectedTo);
        if (!IsConnected(nameof(Moved), connectedTo, "OnMoverMoved"))
            Connect(nameof(Moved), connectedTo, "OnMoverMoved");
        SetProcess(true);
    }


    public override void _Ready() {
        handle = GetNode<ColorRect>("ColorRect");
        handle.Connect("gui_input", this, nameof(OnClick));
        ToolsLayer.activeTool = this;
    }


    public override void _Process(float delta) {
        if (moving) Move();
    }


    public void Move() {
        EmitSignal(nameof(Moved), GetGlobalMousePosition());
        handle.RectGlobalPosition = GetGlobalMousePosition() - handleSize / 2;
    }


    public void OnClick(InputEvent e) {
        if (e is InputEventMouseButton mouseEvent) {
            if (mouseEvent.ButtonIndex == 1) {
                moving = !moving;
                mouseStartPos = GetGlobalMousePosition();
            }
        }
    }
}
