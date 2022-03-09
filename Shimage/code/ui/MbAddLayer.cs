using Godot;
using System;

public class MbAddLayer : MenuButton
{
    Tuple<string, Type>[] layerMenuList = new Tuple<string, Type>[] {
        new Tuple<string, Type>("Image", typeof(LayerImage)),
        new Tuple<string, Type>("Background", typeof(LayerBG)),
        new Tuple<string, Type>("Rectangle", typeof(LayerRect)),
        new Tuple<string, Type>("Text", typeof(LayerText)),
    };
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        int filterIdx = 0;
        foreach (var cLayerEntry in layerMenuList) {
            GetPopup().AddItem(cLayerEntry.Item1);
            filterIdx++;
        }
        GetPopup().Connect("id_pressed", this, nameof(AddLayerFromUI));
    }

    public void AddLayerFromUI(int layeridx) {
        if(Apphandler.currentViewer != null) {
            ILayer layerInstance = (ILayer)Activator.CreateInstance(layerMenuList[layeridx].Item2);
            Apphandler.currentViewer.AddLayer(layerInstance);
        }
    }
}
