using Godot;
using System.Collections.Generic;

public class Apphandler : Node {
    public static Viewer currentViewer;
    public static List<Viewer> openViewers = new List<Viewer>();

    [Export]
    NodePath NPViewTabs;

    [Export]
    NodePath NpViewport;
    [Export]
    public NodePath shaderCodePanel;

    public static Apphandler instance;


    public override void _EnterTree() {
        float UIScale = OS.GetScreenSize().x > 2000 ? 1.15f : 0.8f; // TODO better resolution scaling and DPI Option
        OS.WindowSize = OS.GetScreenSize() * 0.7f; //Always start at 0.7% Resolution
        GetTree().SetScreenStretch(SceneTree.StretchMode.Disabled, SceneTree.StretchAspect.Ignore, new Vector2(128, 128), UIScale);
        instance = this;
    }

    public void RegisterViewer(Viewer viewer) {
        openViewers.Add(viewer);
        GetNode<ViewTabs>(NPViewTabs).RedrawTabs();
    }

    public void Quit() {
        GetTree().Quit();
    }

    public void ShowCode(string code) {
        GetNode<TextEdit>(shaderCodePanel).Text = code;
    }

    public void SetActiveViewer(int idx) {
        currentViewer = GetNode(NpViewport).GetChild<Viewer>(idx);
        foreach (Viewer v in GetNode(NpViewport).GetChildren()) {
            v.Visible = false;
        }
        GetNode(NpViewport).GetChild<Viewer>(idx).Visible = true;
        GetNode<ViewTabs>(NPViewTabs).CurrentTab = idx;
        LayerManager.instance.BuildLayerStackUI();
    }
}