using Godot;

public partial class Rotator : Tool {
    [Signal] delegate void RotatedEventHandler(Vector2 to);
    [Signal] delegate void SStartRotatingEventHandler(Vector2 mouseStartPos);
    [Export]
    Vector2 handleSize = new Vector2(22, 22);
    TextureRect handle;
    bool rotating;
    Vector2 mouseStartPos = new Vector2();

    public void ActivateFromUI() {
        ActivateTool(LayerManager.instance);
        if (!IsConnected(nameof(SStartRotatingEventHandler), new Callable(connectedTo, "OnMoverStarted")))
            Connect(nameof(SStartRotatingEventHandler), new Callable(LayerManager.instance, "OnMoverStarted"));        
    }


    /// <summary> If used, the first element of startPos[] must be Vector2 with global startPosition for the handle </summary>
    public override void ActivateTool(Node connectedTo, Godot.Collections.Array startPos = null) {
        base.ActivateTool(connectedTo);
        if (!IsConnected(nameof(RotatedEventHandler), new Callable(connectedTo, "OnMoverMoved")))
            Connect(nameof(RotatedEventHandler), new Callable(connectedTo, "OnMoverMoved"));

        if (startPos != null) {
            handle.GlobalPosition = (Vector2)startPos[0] - handle.PivotOffset * Apphandler.currentViewer.Scale;
        }
        SetProcess(true);
    }

    public override void DeactivateTool() {
        rotating = false;
        if (connectedTo != null) {

            GD.Print("HUH");
            Disconnect(nameof(RotatedEventHandler), new Callable(connectedTo, "OnMoverMoved"));
            Disconnect(nameof(SStartRotatingEventHandler), new Callable(connectedTo, "OnMoverStarted"));
        }
        base.DeactivateTool();
    }


    public override void _Ready() {
        base._Ready();
        handle = GetNode<TextureRect>("ColorRect");
        handle.Connect("gui_input", new Callable(this, nameof(OnClick)));
        ToolsLayer.activeTool = this;
    }

    public override void OnLayerSelected() {
        base.OnLayerSelected();
        handle.GlobalPosition =  Apphandler.currentViewer.PixelToGlobalCoord((Apphandler.currentViewer.activeLayer as Node2D).Position);
    }


    public override void _Process(double delta) {
        if (rotating) Rotate();
    }


    public void Rotate() {
        EmitSignal(nameof(RotatedEventHandler), GetGlobalMousePosition());
        handle.GlobalPosition = GetGlobalMousePosition() - handle.PivotOffset * Apphandler.currentViewer.Scale;
    }

    public void StartRotating() {
        rotating = !rotating;
        mouseStartPos = GetGlobalMousePosition();
        EmitSignal(nameof(SStartRotatingEventHandler), mouseStartPos);
    }


    public void OnClick(InputEvent e) {
        if (e is InputEventMouseButton mouseEvent) {
            if (mouseEvent.ButtonIndex == MouseButton.Left) {
                StartRotating();
            }
        }
    }
}
