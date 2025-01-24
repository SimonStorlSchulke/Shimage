using Godot;
using System;

public partial class UILayerBG : UILayer {

    ColorPickerButton cpb;

    public override void _Ready() {
        base._Ready();
        cpb = GetNode<ColorPickerButton>("VbLayer/HbColor/ColorPickerButton");
        cpb.ColorChanged += (v) => (Apphandler.currentViewer.Layers[GetIndex()] as LayerBG)?.OnChangeColor(v);
    }
}
