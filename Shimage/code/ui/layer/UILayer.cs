using Godot;
using System;

public class UILayer : Node {

    public bool active = false;
    public bool selected = false;

    OptionButton ObBlendmode;
    Slider SlBlendFactor;

    Color[] statusColors = new Color[3]{new Color(0,0,0,0), new Color("5273d5"), new Color("189dff")};
    public override void _Ready() {
        SetColor();
        ObBlendmode = GetNode<OptionButton>("VbLayer/HbBlend/ObBlendMode");
        SlBlendFactor  = GetNode<Slider>("VbLayer/HbBlend/HsBlendFactor");

        foreach (ShaderUtil.BlendMode item in Enum.GetValues(typeof(ShaderUtil.BlendMode))) {
            ObBlendmode.AddItem(Enum.GetName(typeof(ShaderUtil.BlendMode), item));
        }
        ObBlendmode.Connect("item_selected", Apphandler.currentViewer, nameof(Viewer.OnSetBlendmode), new Godot.Collections.Array(){GetIndex()});
        SlBlendFactor.Connect("value_changed", Apphandler.currentViewer, nameof(Viewer.OnSetBlendFactor), new Godot.Collections.Array(){GetIndex()});
    }

    protected void UpdateSelectionStatus() {
        if (Input.IsKeyPressed((int)KeyList.Shift)) {
            //whith Shift Key
            if (active) {
                selected = active = false;
                SetColor();
                return;
            }
            foreach(Node n in GetParent().GetChildren()) {
                ((UILayer)n).active = false;
                ((UILayer)n).SetColor();
            }
            selected = active = true;
            
        } else {
            //without Shift Key
            foreach(Node n in GetParent().GetChildren()) {
                ((UILayer)n).selected = false;
                ((UILayer)n).active = false;
                ((UILayer)n).SetColor();
            }
            selected = active = true;
        }
        SetColor();
        GetParent<LayerManager>().UpdateLayerSelectionStati();
        FilterManager.BuildFiltersUI();
    }


    public void SetColor() {
        Color c;
        if (active)
            c = statusColors[2];
        else if (selected)
            c = statusColors[1];
        else
            c = statusColors[0];
        GetNode<ColorRect>("ColorFrame").Color = c;
    }


    public void OnInput(InputEvent e) {
        if (e is InputEventMouseButton) {
            if ((e as InputEventMouseButton).ButtonIndex == 1 && (e as InputEventMouseButton).Pressed) {
                UpdateSelectionStatus();
            }
        }
    }

}
