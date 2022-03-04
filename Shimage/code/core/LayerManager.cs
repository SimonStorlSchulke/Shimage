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

    ///<summary>Read selection Status from Layers UI and apply them to Layers</summary>
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

        // Update Selection Colors
        int i = 0;
        foreach (ILayer layer in Apphandler.currentViewer.Layers) {
            if (layer == Apphandler.currentViewer.activeLayer) {
                GetChild<UILayer>(i).active = true;
            } if (Apphandler.currentViewer.selectedLayers.Contains(layer)) {
                GetChild<UILayer>(i).selected = true;
            }
            GetChild<UILayer>(i).SetColor();
            i++;
        }

    }
}
