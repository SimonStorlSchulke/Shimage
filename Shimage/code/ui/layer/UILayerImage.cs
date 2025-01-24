using Godot;
using System;

public partial class UILayerImage : UILayer {

    public void OnFileSelected(string file) {
        GetNode<Label>(NpLblLayerName).Text = file.GetFile();
        GetNode<Label>(NpLblLayerName).TooltipText = file;
        try {
            (GetLayer() as LayerImage).SetTexture(file);
        } catch {

        }
    }
}
