using Godot;
using System.Collections.Generic;

public class LayerManager : Node {
    [Export]
    PackedScene UILayerImage;
    [Export]
    PackedScene UILayerRect;
    [Export]
    PackedScene UILayerText;
    public static LayerManager instance;

    public override void _Ready() {
        instance = this;
    }

    public void UpdateLayerSelectionStati() {

        int i = 0;
        Apphandler.currentViewer.selectedLayers = new List<ILayer>();
        foreach (Node n in GetChildren()) {
            if ((n as UILayer).selected) {
                Apphandler.currentViewer.selectedLayers.Add(Apphandler.currentViewer.Layers[i]);
            } if ((n as UILayer).active) {
                Apphandler.currentViewer.activeLayer = Apphandler.currentViewer.Layers[i];
            }
            i++;
        }
    }

    public void BuildLayerStackUI() {
        Apphandler.currentViewer.CollectLayers(); //TODO call hier? Oder umgekehrt?
        foreach (Node n in GetChildren())
            n.Free();
        foreach (ILayer layer in Apphandler.currentViewer.Layers) {
            Node n;
            if (layer.GetType() == typeof(LayerImage)) {
                n = UILayerImage.Instance();
            } else if(layer.GetType() == typeof(LayerText)) {
                n = UILayerText.Instance();
            } else if (layer.GetType() == typeof(LayerRect)) {
                n = UILayerRect.Instance();
            } else {
                n = UILayerRect.Instance();
            }
            AddChild(n);
        }
    }
}
