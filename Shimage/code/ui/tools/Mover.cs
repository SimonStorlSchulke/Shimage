using Godot;

public class Mover : Tool {
    [Signal] delegate void Moved(Vector2 to);
    [Export]
    Vector2 handleSize = new Vector2(22, 22);
    TextureRect handle;
    bool moving;
    Vector2 mouseStartPos = new Vector2();


    /// <summary> If used, the first element of startPos[] must be Vector2 with global startPosition for the handle </summary>
    public override void ActivateTool(Node connectedTo, Godot.Collections.Array startPos = null) {
        base.ActivateTool(connectedTo);
        if (!IsConnected(nameof(Moved), connectedTo, "OnMoverMoved"))
            Connect(nameof(Moved), connectedTo, "OnMoverMoved");

        if (startPos != null) {
            handle.RectGlobalPosition = (Vector2)startPos[0] - handle.RectPivotOffset * Apphandler.currentViewer.RectScale;
        }
        SetProcess(true);
    }

    public override void DeactivateTool() {
        if (connectedTo != null)
            Disconnect(nameof(Moved), connectedTo, "OnMoverMoved");
        base.DeactivateTool();
    }


    public override void _Ready() {
        handle = GetNode<TextureRect>("ColorRect");
        handle.Connect("gui_input", this, nameof(OnClick));
        ToolsLayer.activeTool = this;
    }


    public override void _Process(float delta) {
        if (moving) Move();
    }


    public void Move() {
        EmitSignal(nameof(Moved), GetGlobalMousePosition());
        handle.RectGlobalPosition = GetGlobalMousePosition() - handle.RectPivotOffset * Apphandler.currentViewer.RectScale; // why??
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
