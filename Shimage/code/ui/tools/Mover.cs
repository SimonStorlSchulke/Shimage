using Godot;

public partial class Mover : Tool {
    [Signal] delegate void MovedEventHandler(Vector2 to);
    [Signal] delegate void SStartedMovingEventHandler(Vector2 mouseStartPos);
    [Export]
    Vector2 handleSize = new Vector2(22, 22);
    TextureRect handle;
    bool moving;
    Vector2 mouseStartPos = new Vector2();

    public void ActivateFromUI() {
        ActivateTool(LayerManager.instance);
        if (!IsConnected(nameof(SStartedMovingEventHandler), new Callable(connectedTo, "OnMoverStarted")))
            Connect(nameof(SStartedMovingEventHandler), new Callable(LayerManager.instance, "OnMoverStarted"));        
    }


    /// <summary> If used, the first element of startPos[] must be Vector2 with global startPosition for the handle </summary>
    public override void ActivateTool(Node connectedTo, Godot.Collections.Array startPos = null) {
        base.ActivateTool(connectedTo);
        if (!IsConnected(nameof(MovedEventHandler), new Callable(connectedTo, "OnMoverMoved")))
            Connect(nameof(MovedEventHandler), new Callable(connectedTo, "OnMoverMoved"));

        if (startPos != null) {
            handle.GlobalPosition = (Vector2)startPos[0] - handle.PivotOffset * Apphandler.currentViewer.Scale;
        }
        SetProcess(true);
    }

    public override void DeactivateTool() {
        moving = false;
        if (connectedTo != null) {

            GD.Print("HUH");
            Disconnect(nameof(MovedEventHandler), new Callable(connectedTo, "OnMoverMoved"));
            Disconnect(nameof(SStartedMovingEventHandler), new Callable(connectedTo, "OnMoverStarted"));
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
        if (moving) Move();
    }


    public void Move() {
        EmitSignal(nameof(MovedEventHandler), GetGlobalMousePosition());
        handle.GlobalPosition = GetGlobalMousePosition() - handle.PivotOffset * Apphandler.currentViewer.Scale;
    }

    public void StartMoving() {
        moving = !moving;
        mouseStartPos = GetGlobalMousePosition();
        EmitSignal(nameof(SStartedMovingEventHandler), mouseStartPos);
    }


    public void OnClick(InputEvent e) {
        if (e is InputEventMouseButton mouseEvent) {
            if (mouseEvent.ButtonIndex == MouseButton.Left) {
                StartMoving();
            }
        }
    }
}
