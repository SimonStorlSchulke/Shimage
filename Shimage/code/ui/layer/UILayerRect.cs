using Godot;
using System;

public class UILayerRect : UILayer {

    ColorPickerButton cpb;

    public override void _Ready() {
        base._Ready();
        cpb = GetNode<ColorPickerButton>("VbLayer/HbColor/ColorPickerButton");
        cpb.GetPicker().Connect(
            "color_changed", 
            Apphandler.currentViewer.Layers[GetIndex()] as LayerRect, 
            nameof(LayerRect.OnChangeColor));
    }
}
