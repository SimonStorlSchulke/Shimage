using Godot;
using System;

public class UILayerImage : UILayer {

    public void OnFileSelected(string file) {
        GetNode<Label>(NpLblLayerName).Text = file.GetFile();
        GetNode<Label>(NpLblLayerName).HintTooltip = file;
        try {
            (GetLayer() as LayerImage).SetTexture(file);
        } catch {

        }
    }
}
