using Godot;
using System.Collections.Generic;

public class Viewer : ViewportContainer {

    [Export]
    NodePath NPViewSpace;
    Control ViewSpacer;
    public ILayer activeLayer;
    public List<ILayer> Layers = new List<ILayer>();
    public List<ILayer> selectedLayers = new List<ILayer>();
    public string projectName = "Unnamed Project";

    public Vector2 Resolution = new Vector2(1920, 1280);

    public override void _Ready() {
        Apphandler.instance.RegisterViewer(this);
        ViewSpacer = GetNode<Control>(NPViewSpace);
        Apphandler.currentViewer = this;
        Recenter();
    }

    public void CollectLayers() {
        Layers = new List<ILayer>();
        foreach(Node n in GetNode("Viewport").GetChildren()) {
            if (n is BackBufferCopy) {
                Layers.Add(n.GetChild<ILayer>(0));
            } else {
                Layers.Add(n as ILayer);
            }
        }
    }

    public void OnSetBlendmode(int idxBlendmode, int idxLayer) {
        Layers[idxLayer].SetBlendmode((ShaderUtil.BlendMode)idxBlendmode);
        Layers[idxLayer].UpdateLayer();
    }

    public void OnSetBlendFactor(float fac, int idxLayer) {
        Layers[idxLayer].SetBlendFactor(fac);
    }

    public void Recenter() {
        float sx = ViewSpacer.RectSize.x / Resolution.x;
        float sy = ViewSpacer.RectSize.y / Resolution.y;
        float fac = Mathf.Min(sx, sy);

        RectPosition = ViewSpacer.RectGlobalPosition;
        RectScale = new Vector2(fac, fac);

        if (sx > sy) {
            float f = ViewSpacer.RectSize.x / 2 - ((Resolution.x * RectScale.x) / 2);
            RectPosition += new Vector2(f, 0);
        } else {
            float f = ViewSpacer.RectSize.y / 2 - ((Resolution.y * RectScale.y) / 2);
            RectPosition += new Vector2(0, f);
        }
    }


    public Vector2 mouseStartPos;
    public Vector2 ViewerStartPos;
    void DragView() {
        RectPosition = ViewerStartPos + GetGlobalMousePosition() - mouseStartPos;
    }


    public bool draggingView;
    public override void _Process(float delta) {
        if (draggingView)
            DragView();
    }


    public void Zoom(float factor) {
        Vector2 mousePosGloabal = ViewSpacer.GetGlobalMousePosition();
        RectScale *= factor;
        RectPosition = mousePosGloabal - (Resolution * RectScale) / 2 + (mousePosGloabal - RectPosition) * factor;
    }
}
