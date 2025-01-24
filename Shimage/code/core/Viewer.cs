using Godot;
using System.Collections.Generic;

public partial class Viewer : SubViewportContainer {

    [Export]
    NodePath NPViewSpace;
    Control ViewSpacer;
    public ILayer activeLayer;
    public List<ILayer> Layers = new List<ILayer>();
    public List<ILayer> selectedLayers = new List<ILayer>();
    public string projectName = "Unnamed Project";

    public Vector2 Resolution = new Vector2(1920, 1280);

    public override void _Ready() {
        Apphandler.RegisterViewer(this);
        ViewSpacer = GetNode<Control>(NPViewSpace);
        Apphandler.currentViewer = this;
        //Recenter();
    }

    public void CollectLayers() {
        Layers = new List<ILayer>();
        foreach(Node n in GetNode("SubViewport").GetChildren()) {
            if (n is BackBufferCopy) {
                Layers.Add(n.GetChild<ILayer>(0));
            } else {
                Layers.Add(n as ILayer);
            }
        }
    }

    public void AddLayer(ILayer layer) {
        Layers.Add(layer);
        GD.Print("ADD", layer.GetType());
        LayerManager.instance.AddLayerUI(layer);

        if (GetNode("SubViewport").GetChildCount() > 1) {
            BackBufferCopy bbc = new BackBufferCopy();
            bbc.AddChild(layer as Node);
             GetNode("SubViewport").AddChild(bbc);
             return;
        }
        GetNode("SubViewport").AddChild(layer as Node);
    }

    public void OnSetBlendmode(int idxBlendmode, int idxLayer) {
        Layers[idxLayer].SetBlendmode((ShaderUtil.BlendMode)idxBlendmode);
        Layers[idxLayer].UpdateLayer();
    }

    public void OnSetBlendFactor(float fac, int idxLayer) {
        Layers[idxLayer].SetBlendFactor(fac);
    }

    public void Recenter() {
        float sx = ViewSpacer.Size.X / Resolution.X;
        float sy = ViewSpacer.Size.Y / Resolution.Y;
        float fac = Mathf.Min(sx, sy);

        Position = ViewSpacer.GlobalPosition;
        Scale = new Vector2(fac, fac);

        if (sx > sy) {
            float f = ViewSpacer.Size.X / 2 - ((Resolution.X * Scale.X) / 2);
            Position += new Vector2(f, 0);
        } else {
            float f = ViewSpacer.Size.Y / 2 - ((Resolution.Y * Scale.Y) / 2);
            Position += new Vector2(0, f);
        }
        ToolsLayer.UpdateTransform();
    }


    public Vector2 mouseStartPos;
    public Vector2 ViewerStartPos;
    void DragView() {
        Position = ViewerStartPos + GetGlobalMousePosition() - mouseStartPos;
        ToolsLayer.UpdateTransform();
    }


    public bool draggingView;
    public override void _Process(double delta) {
        if (draggingView)
            DragView();
    }


    public void Zoom(float factor) {
        Vector2 mousePosGloabal = ViewSpacer.GetGlobalMousePosition();
        Scale *= factor;
        Position = mousePosGloabal - (Resolution * Scale) / 2 + (mousePosGloabal - Position) * factor;
        ToolsLayer.UpdateTransform();
    }

    public Vector2 GlobalToPixelCoord(Vector2 globalCords) {
        return (globalCords - GlobalPosition) / Scale;
    }

    public Vector2 PixelToGlobalCoord(Vector2 pixelCoords) {
        return Scale * pixelCoords + GlobalPosition;
    }

    public Vector2 GlobalToUVCoord(Vector2 globalCords) {
        return GlobalToPixelCoord(globalCords) / Resolution;
    }
}
