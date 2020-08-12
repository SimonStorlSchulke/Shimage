using Godot;
using System;

public class Viewer : Sprite {

    [Export]
    float zoomSpeed = 0.1f;

    ImageTexture tex = new ImageTexture();
    Image img = new Image();

    public override void _Ready() {

        //var Aspect = new Vector2(Texture.GetWidth(), Texture.GetHeight()).Normalized();

        //Example image Manipulation
        img.Load("res://TestImages/kylo.png");

        PointFilter f = new ExampleFilter(new Color(1, 0, 0));
        f.Apply(img);

        tex.CreateFromImage(img);

        this.Texture = tex;

    }

    public override void _Input(InputEvent e) {
        if (e.IsAction("zoom_in"))
            Scale *= (1 + zoomSpeed);
        
        if (e.IsAction("zoom_out"))
            Scale *= (1 - zoomSpeed);
        
    }
}