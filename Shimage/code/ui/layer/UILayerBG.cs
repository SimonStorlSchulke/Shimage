using Godot;
using System;

public class UILayerBG : UILayer {

    ColorPickerButton cpb;

    public override void _Ready() {
        base._Ready();
        cpb = GetNode<ColorPickerButton>("VbLayer/HbColor/ColorPickerButton");
        cpb.GetPicker().Connect(
            "color_changed", 
            Apphandler.currentViewer.Layers[GetIndex()] as LayerBG, 
            nameof(LayerBG.OnChangeColor));
    }
}
