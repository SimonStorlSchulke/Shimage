using Godot;
using System;

public class Viewer : Sprite {

    [Export]
    float zoomSpeed = 0.1f;

    ImageTexture tex = new ImageTexture();
    Image img = new Image();

    public static Viewer instance = null;

    public override void _Ready() {
        if (instance == null) {
            instance = this;
        }
        GD.PushError("Only one Viewer instance is supported cuurently");

        img.Load("res://TestImages/kylo.png");
        tex.CreateFromImage(img);
        Texture = tex;
    }


    public void OnApplyFilters() {

        foreach (PointFilter filter in FilterStack.filterList)
        {
            filter.Apply(img);
        }

        tex.CreateFromImage(img);
        Texture = tex;
    }

    public override void _Input(InputEvent e) {
        if (e.IsAction("zoom_in"))
            Scale *= (1 + zoomSpeed);
        
        if (e.IsAction("zoom_out"))
            Scale *= (1 - zoomSpeed);
    }
}