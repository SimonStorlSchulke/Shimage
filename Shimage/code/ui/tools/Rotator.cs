using Godot;

public class Rotator : Tool {
    [Signal] delegate void Rotated(Vector2 to);
    [Signal] delegate void SStartRotating(Vector2 mouseStartPos);
    [Export]
    Vector2 handleSize = new Vector2(22, 22);
    TextureRect handle;
    bool rotating;
    Vector2 mouseStartPos = new Vector2();

    public void ActivateFromUI() {
        ActivateTool(LayerManager.instance);
        if (!IsConnected(nameof(SStartRotating), connectedTo, "OnMoverStarted"))
            Connect(nameof(SStartRotating), LayerManager.instance, "OnMoverStarted");        
    }


    /// <summary> If used, the first element of startPos[] must be Vector2 with global startPosition for the handle </summary>
    public override void ActivateTool(Node connectedTo, Godot.Collections.Array startPos = null) {
        base.ActivateTool(connectedTo);
        if (!IsConnected(nameof(Rotated), connectedTo, "OnMoverMoved"))
            Connect(nameof(Rotated), connectedTo, "OnMoverMoved");

        if (startPos != null) {
            handle.RectGlobalPosition = (Vector2)startPos[0] - handle.RectPivotOffset * Apphandler.currentViewer.RectScale;
        }
        SetProcess(true);
    }

    public override void DeactivateTool() {
        rotating = false;
        if (connectedTo != null) {

            GD.Print("HUH");
            Disconnect(nameof(Rotated), connectedTo, "OnMoverMoved");
            Disconnect(nameof(SStartRotating), connectedTo, "OnMoverStarted");
        }
        base.DeactivateTool();
    }


    public override void _Ready() {
        base._Ready();
        handle = GetNode<TextureRect>("ColorRect");
        handle.Connect("gui_input", this, nameof(OnClick));
        ToolsLayer.activeTool = this;
    }

    public override void OnLayerSelected() {
        base.OnLayerSelected();
        handle.RectGlobalPosition =  Apphandler.currentViewer.PixelToGlobalCoord((Apphandler.currentViewer.activeLayer as Node2D).Position);
    }


    public override void _Process(float delta) {
        if (rotating) Rotate();
    }


    public void Rotate() {
        EmitSignal(nameof(Rotated), GetGlobalMousePosition());
        handle.RectGlobalPosition = GetGlobalMousePosition() - handle.RectPivotOffset * Apphandler.currentViewer.RectScale;
    }

    public void StartRotating() {
        rotating = !rotating;
        mouseStartPos = GetGlobalMousePosition();
        EmitSignal(nameof(SStartRotating), mouseStartPos);
    }


    public void OnClick(InputEvent e) {
        if (e is InputEventMouseButton mouseEvent) {
            if (mouseEvent.ButtonIndex == 1) {
                StartRotating();
            }
        }
    }
}
