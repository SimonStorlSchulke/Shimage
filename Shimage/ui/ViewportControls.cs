using Godot;
using System;

public partial class ViewportControls : Control {
    public void OnInput(InputEvent e) {
        if (e.IsAction("zoom_in")) {
            Apphandler.currentViewer.Zoom(1.2f);
        }

        if (e.IsAction("zoom_out")) {
            Apphandler.currentViewer.Zoom(0.9f);
        }

        //Drag Viewer
        if (e is InputEventMouseButton) {
            if ((e as InputEventMouseButton).ButtonIndex == MouseButton.Middle) {
                Apphandler.currentViewer.draggingView = !Apphandler.currentViewer.draggingView;
                Apphandler.currentViewer.mouseStartPos = GetGlobalMousePosition();
                Apphandler.currentViewer.ViewerStartPos = Apphandler.currentViewer.Position;
            }
        }
    }

    public void OnRecenter() {
        Apphandler.currentViewer.Recenter();
    }
}
