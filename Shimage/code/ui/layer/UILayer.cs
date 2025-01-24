using Godot;
using System;

public partial class UILayer : Node {

    public bool active = false;
    public bool selected = false;

    OptionButton ObBlendmode;
    Slider SlBlendFactor;
    [Export] protected NodePath NpLblLayerName;

    Color[] statusColors = new Color[3]{new Color(0,0,0,0), new Color("5273d5"), new Color("189dff")};
    public override void _Ready() {
        SetColor();
        ObBlendmode = GetNode<OptionButton>("VbLayer/HbBlend/ObBlendMode");
        SlBlendFactor  = GetNode<Slider>("VbLayer/HbBlend/HsBlendFactor");

        foreach (ShaderUtil.BlendMode item in Enum.GetValues(typeof(ShaderUtil.BlendMode))) {
            ObBlendmode.AddItem(Enum.GetName(typeof(ShaderUtil.BlendMode), item));
        }

        ObBlendmode.ItemSelected += (v) => Apphandler.currentViewer.OnSetBlendmode((int)v, GetIndex()); 
        SlBlendFactor.ValueChanged += (v) => Apphandler.currentViewer.OnSetBlendFactor((int)v, GetIndex()); 
    }

    ///<summary> Get the Layer correcsponing to this UI </summary>
    protected ILayer GetLayer() {
        return Apphandler.currentViewer.Layers[GetIndex()];
    }

    protected void UpdateSelectionStatus() {
        if (Input.IsKeyPressed(Key.Shift)) {
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
            if ((e as InputEventMouseButton).ButtonIndex == MouseButton.Left && (e as InputEventMouseButton).Pressed) {
                UpdateSelectionStatus();
            }
        }
    }

}
